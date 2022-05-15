﻿using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public sealed class FinancialCardMap : ClassMap<FinancialCard>
    {
        public FinancialCardMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            References(x => x.Employee);
            #region Finance Details

            Map(x => x.Salary);
            Map(x => x.InsuranceSalary);
            Map(x => x.TempSalary1);
            Map(x => x.TempSalary2);
            Map(x => x.BenefitSalary);
            Map(x => x.Threshold);
            Map(x => x.SalaryDeservableType);
            Map(x => x.ProbationPeriodPercentage);
            References(x => x.CostCenter);
            References(x => x.CurrencyType);

            #endregion
        }
    }
}
