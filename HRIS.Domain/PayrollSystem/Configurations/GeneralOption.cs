using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Helpers;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    [Order(70)]
    [Module(ModulesNames.PayrollSystem)]
    public class GeneralOption : Entity, IConfigurationRoot
    {

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 5)]
        public virtual int TotalMonthDays { get; set; } // عدد ايام الدوام بالشهر

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 10)]
        public virtual double TotalDayHours { get; set; } // عدد ساعات الدوام في اليوم

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 15)]
        public virtual double TaxThreshold { get; set; } // الحد الادنى المعفى من الضريبة

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 20)]
        public virtual bool StoppingTaxByReserveMilitaryService { get; set; } // حالة الخدمة العسكرية تؤثر بالضريبة بحيث يتم ايقاف الضريبة اذا كان الموظف مسحوب للاحتياط

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BenefitsDetails, Order = 25, IsReference = true)]
        public virtual BenefitCard RewardBenefit { get; set; } // تعويض المكافأة 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BenefitsDetails, Order = 35, IsReference = true)]
        public virtual BenefitCard OvertimeBenefit { get; set; } // تعويض الدوام الإضافي

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BenefitsDetails, Order = 40, IsReference = true)]
        public virtual BenefitCard RecycledLeaveBenefit { get; set; } // تعويض تدوير الإجازات المالي السنوي

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.DeductionsDetails, Order = 45, IsReference = true)]
        public virtual DeductionCard TaxDeduction { get; set; } // حسم الضريبة 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.DeductionsDetails, Order = 50, IsReference = true)]
        public virtual DeductionCard LeaveDeduction { get; set; } // حسم الاجازة  

        //[UserInterfaceParameter(Order = 50, IsReference = true)]
        //public virtual DeductionCard HealthyLeaveDeduction { get; set; } // حسم حسم الاجازة المرضية 

        //[UserInterfaceParameter(Order = 55, IsReference = true)]
        //public virtual DeductionCard UnpaidMaternityLeaveDeduction { get; set; } // حسم إجازة الامومة الاضافية بلا أجر 

        //[UserInterfaceParameter(Order = 60, IsReference = true)]
        //public virtual DeductionCard MaternityLeaveDeduction { get; set; } // حسم إجازة الامومة 

        //[UserInterfaceParameter(Order = 65, IsReference = true)]
        //public virtual DeductionCard AdministrativeLeaveDeduction { get; set; } // حسم الاجازة الادراية 

        //[UserInterfaceParameter(Order = 70, IsReference = true)]
        //public virtual DeductionCard UnpaidLeaveDeduction { get; set; } // حسم الاجازة بلا أجر 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.DeductionsDetails, Order = 55, IsReference = true)]
        public virtual DeductionCard PenaltyDeduction { get; set; } // حسم العقوبة 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.DeductionsDetails, Order = 60, IsReference = true)]
        public virtual DeductionCard AbsenceDaysDeduction { get; set; } // حسم أيام الغياب 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.DeductionsDetails, Order = 65, IsReference = true)]
        public virtual DeductionCard NonAttendanceDeduction { get; set; } // حسم نقص الدوام 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.DeductionsDetails, Order = 70, IsReference = true)]
        public virtual DeductionCard LatenessDeduction { get; set; } // حسم التأخير الصباحي 

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AdvanceDetails, Order = 75)]
        public virtual int AdvanceDeductionDaysFromEmployeeAttendance { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AdvanceDetails, Order = 80)]
        public virtual bool AdvanceDependesOnFromDateToDate { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AdvanceDetails, Order = 85)]
        public virtual int AdvanceFromDate { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AdvanceDetails, Order = 90)]
        public virtual int AdvanceToDate { get; set; }


        #region salary packages details

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SalaryPackages, Order = 95)]
        public virtual bool Salary { get; set; } // راتب الموظف المقطوع

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SalaryPackages, Order = 100)]
        public virtual bool BenefitSalary { get; set; } // 1راتب الموظف الاحتياطي

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SalaryPackages, Order = 115)]
        public virtual bool InsuranceSalary { get; set; } // راتب الموظف التأميني

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SalaryPackages, Order = 120)]
        public virtual bool TempSalary1 { get; set; } // راتب الموظف الاحتياطي2

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SalaryPackages, Order = 135)]
        public virtual bool TempSalary2 { get; set; } // راتب الموظف الاحتياطي3

        #endregion

        //[UserInterfaceParameter(Order = 75)]
        //public virtual bool AllowAuditFeature { get; set; } // الخيار الذي يسمح بميزة التدقيق في النظام من عدمها

    }
}
