using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training.Entities
{

    public class CareerPathNode : Entity
    {
        /// <summary>
        /// المسمى الوظيفي
        /// </summary>
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual JobTitle JobTitle { get; set; }
        /// <summary>
        /// الوصف الوظيفي
        /// </summary>
        [UserInterfaceParameter(Order = 2, IsReference = true, CascadeFrom = "JobTitle", ReferenceReadUrl = "Training/Reference/ReadJobDescriptionCascadeJobTitle")]
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
        /// <summary>
        /// الخطوات ضمن الفئة
        /// </summary>
        [UserInterfaceParameter(Order = 3, IsReference = true, CascadeFrom = "JobDescription", ReferenceReadUrl = "Training/Reference/ReadGradeStepCascadeJobTitle")]
        public virtual GradeStep GradeStep { get; set; }
        public virtual CareerPathFamily CareerPathFamily { get; set; }
    }
}
