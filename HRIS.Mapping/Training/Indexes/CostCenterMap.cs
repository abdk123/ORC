using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Indexes;
using Souccar.Core;

namespace HRIS.Mapping.Training.Indexes
{
    public class CostCenterMap : ClassMap<CostCenter>
    {
        public CostCenterMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
