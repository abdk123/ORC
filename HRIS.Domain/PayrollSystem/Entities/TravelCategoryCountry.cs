using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Indexes;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class TravelCategoryCountry : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual TravelCategory TravelCategory { get; set; }

        [UserInterfaceParameter(Order = 10, IsReference = true)]
        public virtual Country Country { get; set; }

    }
}
