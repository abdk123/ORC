using System;
using System.ComponentModel.DataAnnotations;
using Souccar.Core;
using Souccar.Core.DesignByContract;
using Souccar.Domain;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;

namespace Souccar.NHibernate.NHibernateValidator
{
    /// <summary>
    ///     Provides a class level validator for determining if the entity has a unique domain signature
    ///     when compared with other entries in the database.
    /// 
    ///     Due to the fact that .NET does not support generic attributes, this only works for entity 
    ///     types having an Id of type int.
    /// </summary> 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HasUniqueDomainSignatureAttribute : ValidationAttribute
    {
        public HasUniqueDomainSignatureAttribute()
            : base("Provided values matched an existing, duplicate entity")
        {
        }

        public override bool IsValid(object value)
        {
            var entityToValidate = value as IEntityWithTypedId<int>;
            Check.Require(
                entityToValidate != null,
                "This validator must be used at the class level of an IEntityWithTypedId<int>. The type you provided was " +
                value.GetType());

            IEntityDuplicateChecker duplicateChecker = SafeServiceLocator<IEntityDuplicateChecker>.GetService();
            return ! duplicateChecker.DoesDuplicateExistWithTypedIdOf(entityToValidate);
        }
    }
}