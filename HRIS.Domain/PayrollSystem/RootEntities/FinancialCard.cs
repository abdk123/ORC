using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Module(ModulesNames.PayrollSystem)]
    public class FinancialCard : Entity, IAggregateRoot
    {
        public FinancialCard()
        {

        }

        [UserInterfaceParameter(Order = 10, IsReference = true, IsNonEditable =true)]
        public virtual Employee Employee { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual string FatherName {
            get
            {
                return Employee != null ? Employee.FatherName : "";
            }
        }
        [UserInterfaceParameter(Order =55)]
        public virtual float PackageSalary { get {
                var options = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
                if(options != null)
                {
                    var _packageSalary = options.Salary ? Salary : 0;
                    _packageSalary = options.BenefitSalary ? _packageSalary + BenefitSalary : _packageSalary;
                    _packageSalary = options.TempSalary1 ? _packageSalary + TempSalary1 : _packageSalary;
                    _packageSalary = options.TempSalary2 ? _packageSalary + TempSalary2 : _packageSalary;
                    _packageSalary = options.InsuranceSalary ? _packageSalary + InsuranceSalary : _packageSalary;
                    return _packageSalary;
                }
                return 0;
            }
        }
        #region Finance Details (Payroll System)

        [UserInterfaceParameter(Order = 60)]
        public virtual CostCenter CostCenter { get; set; }

        [UserInterfaceParameter(Order = 65)]
        public virtual float Salary { get; set; } // راتب الموظف المقطوع

        [UserInterfaceParameter(Order = 70)]
        public virtual float BenefitSalary { get; set; } // 1راتب الموظف الاحتياطي

        [UserInterfaceParameter(Order = 75)]
        public virtual float InsuranceSalary { get; set; } // راتب الموظف التأميني

        [UserInterfaceParameter(Order = 80)]
        public virtual float TempSalary1 { get; set; } // راتب الموظف الاحتياطي2

        [UserInterfaceParameter(Order = 85)]
        public virtual float TempSalary2 { get; set; } // راتب الموظف الاحتياطي3

        [UserInterfaceParameter(Order = 90)]
        public virtual float Threshold { get; set; } // عتبة الراتب

        [UserInterfaceParameter(Order = 95)]
        public virtual CurrencyType CurrencyType { get; set; }

        /// <summary>
        /// نسبة الراتب في فترة الإختبار
        /// </summary>
        [UserInterfaceParameter(Order = 100)]
        public virtual float ProbationPeriodPercentage { get; set; }

        [UserInterfaceParameter(Order = 105)]
        public virtual SalaryDeservableType SalaryDeservableType { get; set; }



        #endregion
    }
}
