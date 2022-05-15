using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Status = HRIS.Domain.Global.Enums.Status;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
{

    public static class PayrollIntegrationService
    {

        #region Employee Relation Services

        public static List<PayrollSystemIntegrationDTO> GetLeaves(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // الاجازات

            var result = new List<PayrollSystemIntegrationDTO>();
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);

            if (employeeCard != null)
            {
                List<LeaveRequest> leaveRequests;

                if (!fromDate.HasValue && !toDate.HasValue)
                    leaveRequests = employeeCard.LeaveRequests.Where(x => x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted).ToList();
                else if (fromDate.HasValue && toDate.HasValue)
                    leaveRequests = employeeCard.LeaveRequests.
                        Where(x => x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted &&
                        ((x.StartDate <= fromDate && x.EndDate >= toDate) ||
                         (x.StartDate <= toDate && x.EndDate >= toDate) ||
                         (x.StartDate <= fromDate && x.EndDate >= fromDate) ||
                         (x.StartDate >= fromDate && x.EndDate <= toDate)))
                        .ToList();
                else if (fromDate.HasValue)
                    leaveRequests = employeeCard.LeaveRequests.
                        Where(x => x.StartDate == fromDate && x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted).ToList();
                else
                    leaveRequests = employeeCard.LeaveRequests.
                        Where(x => x.EndDate == toDate && x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted).ToList();

                if (leaveRequests.Count > 0)
                {
                    var leaveRequestsGroupedByLeaveType = leaveRequests.GroupBy(x => x.LeaveSetting);
                    foreach (var item in leaveRequestsGroupedByLeaveType)
                    {
                        var ids = new List<int>();
                        var order = 0;
                        var spentDaysValueForDeduction = 0.00;
                        var leaveSetting = item.Key;
                        var leavesRequested = item.ToList();
                        double totalLeaveSpentDays = 0;
                        double extraValues = 0;
                        foreach (var leaveRequest in leavesRequested)
                        {
                            ids.Add(leaveRequest.Id);
                            double leaveSpentDays = fromDate.HasValue && toDate.HasValue &&
                                (leaveRequest.StartDate < fromDate.Value || leaveRequest.EndDate > toDate.Value) ?
                                GetSpentDaysBetweenTwoDates(leaveRequest, fromDate, toDate) : leaveRequest.SpentDays;
                            totalLeaveSpentDays += leaveSpentDays;
                        }
                        if (leaveSetting.PaidSlices != null && leaveSetting.PaidSlices.Count > 0)
                        {
                            foreach (var pidSlice in leaveSetting.PaidSlices.OrderBy(x => x.FromBalance))
                            {
                                order += 1;
                                extraValues = pidSlice.PaidPercentage;
                                if (totalLeaveSpentDays == 0)
                                    break;
                                if (pidSlice.ToBalance > totalLeaveSpentDays)
                                {
                                    spentDaysValueForDeduction = totalLeaveSpentDays;
                                    totalLeaveSpentDays = 0;
                                }
                                else
                                {
                                    spentDaysValueForDeduction = pidSlice.ToBalance;
                                    totalLeaveSpentDays -= pidSlice.ToBalance;
                                }
                                result.Add(new PayrollSystemIntegrationDTO
                                {
                                    Value = spentDaysValueForDeduction,
                                    Formula = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard.Formula : Formula.DaysOfPackageSalary,
                                    ExtraValue = -extraValues,
                                    ExtraValueFormula = ExtraValueFormula.PercentageOfInitialValue,
                                    Repetition = 1,
                                    SourceId = ids,
                                    SourceType = 0,
                                    Note = leaveSetting.NameForDropdown + " " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AccordingToLeavePaidSliceWhichOrderIs) + order,
                                    DeductionCard = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard : null
                                });

                            }
                        }
                        else
                        {
                            extraValues = leaveSetting.PaidPercentage;
                            result.Add(new PayrollSystemIntegrationDTO
                            {
                                Value = totalLeaveSpentDays,
                                Formula = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard.Formula : Formula.DaysOfPackageSalary,
                                ExtraValue = -extraValues,
                                ExtraValueFormula = ExtraValueFormula.PercentageOfInitialValue,
                                Repetition = 1,
                                SourceId = ids,
                                SourceType = 0,
                                Note = leaveSetting.NameForDropdown,
                                DeductionCard = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard : null
                            });
                        }
                    }

                }
            }

            return result;
        }

        private static double GetSpentDaysBetweenTwoDates(LeaveRequest leaveRequest, DateTime? fromDate, DateTime? toDate)
        {
            var spentDays = LeaveService.GetSpentDays(leaveRequest.StartDate >= fromDate.Value ? leaveRequest.StartDate : fromDate.Value,
                leaveRequest.EndDate <= toDate.Value ? leaveRequest.EndDate : toDate.Value, leaveRequest.LeaveSetting.IsContinuous, leaveRequest.EmployeeCard.Employee);
            return spentDays;
        }

        public static List<PayrollSystemIntegrationDTO> GetRecycledLeaves(Employee employee, bool isAccepted)
        {
            // الاجازات السنوية المدورة مالياً

            var recycledLeaves = new List<RecycledLeave>();
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                recycledLeaves = employeeCard.RecycledLeaves.Where(
                    x => x.RecycleType == RecycleType.Salary && x.IsTransferToPayroll == isAccepted).ToList();
            }

            var result = new List<PayrollSystemIntegrationDTO>();

            result.AddRange(recycledLeaves
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RoundedBalance,
                    Formula = Formula.DaysOfSalary,
                    Repetition = 1,
                    SourceId = new List<int>() { x.Id }
                }).ToList());

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetPenalties(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // العقوبات

            List<EmployeeDisciplinary> penalties;

            if (!fromDate.HasValue && !toDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.Where(x => x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate >= fromDate && x.DisciplinaryDate <= toDate && x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate >= fromDate && x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate <= toDate && x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();

            var result = penalties
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.DisciplinarySetting.Value,
                    Formula = x.DisciplinarySetting.DeductionType == DeductionType.PercentageOfPackageSalary ? Formula.PercentageOfPackageSalary :
                    x.DisciplinarySetting.DeductionType == DeductionType.DaysOfPackageSalary ? Formula.DaysOfPackageSalary :
                    Formula.FixedValue,
                    Repetition = 1,
                    SourceId = new List<int>() { x.Id }
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetRewards(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // المكافآت

            List<EmployeeReward> rewards;

            if (!fromDate.HasValue && !toDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.Where(x => x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate >= fromDate && x.RewardDate <= toDate && x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate >= fromDate && x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate <= toDate && x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();

            var result = rewards
               .Select(x => new PayrollSystemIntegrationDTO
               {
                   Value = x.RewardSetting.IsPercentage ? x.RewardSetting.Percentage : x.RewardSetting.FixedValue,
                   Formula = x.RewardSetting.IsPercentage ? Formula.PercentageOfSalary : Formula.FixedValue,
                   Repetition = 1,
                   SourceId = new List<int>() { x.Id }
               }).ToList();

            return result;
        }

        public static IList<LeaveRequest> AcceptLeave(Employee employee, MonthlyEmployeeDeduction monthlyEmplyeeDeduction)
        {
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                var leaves = employeeCard.LeaveRequests.Where(x => monthlyEmplyeeDeduction.LeaveDeductions.Any(y => y.LeaveId == x.Id));
                if (!leaves.Any())
                    return new List<LeaveRequest>();
                foreach (var leave in leaves)
                {
                    leave.IsTransferToPayroll = monthlyEmplyeeDeduction.MonthlyCard.Month.ToDate < leave.EndDate ? false : true;
                }
                return leaves.ToList();
            }
            return new List<LeaveRequest>();
        }
        public static RecycledLeave AcceptRecycledLeave(Employee employee, int sourceId)
        {
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                var recycledLeave = employeeCard.RecycledLeaves.FirstOrDefault(x => x.Id == sourceId);
                if (recycledLeave == null)
                    return null;
                recycledLeave.IsTransferToPayroll = true;
                return recycledLeave;
            }
            return null;
        }
        public static EmployeeDisciplinary AcceptPenalty(Employee employee, int sourceId)
        {
            var penalty = employee.EmployeeCard.EmployeeDisciplinarys.FirstOrDefault(x => x.Id == sourceId);
            if (penalty == null)
                return null;
            penalty.IsTransferToPayroll = true;
            return penalty;
        }
        public static List<IAggregateRoot> AcceptAdvances(Employee employee)
        {
            var advances = employee.EmployeeCard.EmployeeAdvances.Where(x => x.SourceId > 0).ToList();
            var entities = new List<IAggregateRoot>();
            if (advances.Count() == 0)
                return entities;
            foreach (var advance in advances)
            {
                advance.IsTransferToPayroll = true;
                entities.Add(advance);
                return entities;
            }
            return entities;
        }
        public static EmployeeReward AcceptReward(Employee employee, int sourceId)
        {
            var reward = employee.EmployeeCard.EmployeeRewards.FirstOrDefault(x => x.Id == sourceId);
            if (reward == null)
                return null;
            reward.IsTransferToPayroll = true;
            return reward;
        }

        #endregion

        #region Attendance

        public enum AttendanceType
        {
            Overtime = 0,
            Absence = 1,
            NonAttendance = 2,
            Lateness = 3
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attendanceType"></param>
        /// <param name="employee"></param>
        /// <param name="date"></param>
        /// <param name="isAccepted"></param>
        /// <returns></returns>
        public static List<PayrollSystemIntegrationDTO> ImportFromAttendance(AttendanceType attendanceType, Employee employee, DateTime fromDate, DateTime toDate, bool isAccepted)
        {
            // الدوام الإضافي

            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => x.FromDate == fromDate && x.ToDate == toDate && x.AttendanceMonthStatus == AttendanceMonthStatus.Locked);
            var payrollSystemIntegrationDtOs = new List<PayrollSystemIntegrationDTO>();

            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment = GetAttendanceWithoutAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceMonthlyAdjustment = GetAttendanceMonthlyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceDailyAdjustment = GetAttendanceDailyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);

                if (attendanceWithoutAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceWithoutAdjustment(attendanceWithoutAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null && payrollSystemIntegrationDTO.Value > 0)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceMonthlyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceMonthlyAdjustment(attendanceMonthlyAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null && payrollSystemIntegrationDTO.Value > 0)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceDailyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceDailyAdjustment(attendanceDailyAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null && payrollSystemIntegrationDTO.Value > 0)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }
            }

            return payrollSystemIntegrationDtOs;
        }
        private static AttendanceWithoutAdjustment GetAttendanceWithoutAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static AttendanceMonthlyAdjustment GetAttendanceMonthlyAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static AttendanceDailyAdjustment GetAttendanceDailyAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceWithoutAdjustment(AttendanceWithoutAdjustment attendanceWithoutAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId = new List<int>() { attendanceWithoutAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        if (attendanceWithoutAdjustment.FinalTotalOvertimeValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalTotalOvertimeValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        if (attendanceWithoutAdjustment.TotalAbsenceDaysValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.TotalAbsenceDaysValue;
                            payrollSystemIntegrationDTO.Formula = Formula.DaysOfSalary;
                        }
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        if (attendanceWithoutAdjustment.FinalNonAttendanceTotalValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalNonAttendanceTotalValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        if (attendanceWithoutAdjustment.FinalLatenessTotalValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalLatenessTotalValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceMonthlyAdjustment(AttendanceMonthlyAdjustment attendanceMonthlyAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId = new List<int>() { attendanceMonthlyAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        if (attendanceMonthlyAdjustment.FinalOvertimeValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalOvertimeValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        if (attendanceMonthlyAdjustment.FinalNonAttendanceValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalNonAttendanceValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceDailyAdjustment(AttendanceDailyAdjustment attendanceDailyAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId = new List<int>() { attendanceDailyAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        if (attendanceDailyAdjustment.FinalOvertimeValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalOvertimeValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        if (attendanceDailyAdjustment.TotalAbsenceDays > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.TotalAbsenceDays;
                            payrollSystemIntegrationDTO.Formula = Formula.DaysOfSalary;
                        }
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        if (attendanceDailyAdjustment.FinalNonAttendanceValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalNonAttendanceValue;
                            payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        }
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        public static void AcceptAttendance(AttendanceRecord attendanceRecord, AttendanceType attendanceType, int sourceId)
        {

            if (attendanceRecord != null)
            {
                AcceptAttendanceWithoutAdjustment(attendanceType, attendanceRecord, sourceId);
                AcceptAttendanceMonthlyAdjustment(attendanceType, attendanceRecord, sourceId);
                AcceptAttendanceDailyAdjustment(attendanceType, attendanceRecord, sourceId);
            }
        }
        private static void AcceptAttendanceWithoutAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment =
                    attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceWithoutAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceWithoutAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceWithoutAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceWithoutAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceWithoutAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

            }

        }
        private static void AcceptAttendanceMonthlyAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceMonthlyAdjustment =
                    attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceMonthlyAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceMonthlyAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceMonthlyAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceMonthlyAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceMonthlyAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

            }



        }
        private static void AcceptAttendanceDailyAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceDailyAdjustment =
                    attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceDailyAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceDailyAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceDailyAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceDailyAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceDailyAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }
            }



        }

        #endregion

    }


}