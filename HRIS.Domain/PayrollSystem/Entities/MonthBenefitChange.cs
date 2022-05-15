using System.Collections.Generic;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class MonthBenefitChange : EmployeeBenefitBase
    {
        public MonthBenefitChange()
        {
            MonthlyEmployeeBenefits = new List<MonthlyEmployeeBenefit>();
        }

        [UserInterfaceParameter(Order = 5)]
        public virtual Month Month { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual ConflictOption ConflictOption { get; set; }

        public virtual IList<MonthlyEmployeeBenefit> MonthlyEmployeeBenefits { get; set; }
        public virtual void AddMonthlyEmployeeBenefit(MonthlyEmployeeBenefit monthlyEmployeeBenefit)
        {
            MonthlyEmployeeBenefits.Add(monthlyEmployeeBenefit);
            monthlyEmployeeBenefit.MonthBenefitChange = this;
        }
    }
}
