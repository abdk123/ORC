#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
#endregion
namespace HRIS.Domain.Personnel.Indexes
{
    [Module(ModulesNames.Personnel)]
    public class CustodiesType : IndexEntity, IAggregateRoot
    {
    }
}