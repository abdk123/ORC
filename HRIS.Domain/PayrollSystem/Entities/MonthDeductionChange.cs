using System.Collections.Generic;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class MonthDeductionChange : EmployeeDeductionBase
    {
        public MonthDeductionChange()
        {
            MonthlyEmployeeDeductions = new List<MonthlyEmployeeDeduction>();
        }
        [UserInterfaceParameter(Order = 5)]
        public virtual Month Month { get; set; }


        [UserInterfaceParameter(Order = 40)]
        public virtual ConflictOption ConflictOption { get; set; }

        public virtual IList<MonthlyEmployeeDeduction> MonthlyEmployeeDeductions { get; set; }
        public virtual void AddMonthlyEmployeeDeduction(MonthlyEmployeeDeduction monthlyEmployeeDeduction)
        {
            MonthlyEmployeeDeductions.Add(monthlyEmployeeDeduction);
            monthlyEmployeeDeduction.MonthDeductionChange = this;
        }
    }
}
