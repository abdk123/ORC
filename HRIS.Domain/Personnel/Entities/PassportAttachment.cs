﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;

namespace HRIS.Domain.Personnel.Entities
{
    public class PassportAttachment : Attachment
    {

      
        public virtual Passport Passport { get; set; }
    }
}
