
using Repositories.Enums;
using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class PROJECT
    {
        public virtual long ID { get; set; }
        public virtual int PROJECT_NUMBER { get; set; }
        public virtual string NAME { get; set; }
        public virtual string CUSTOMER { get; set; }
        public virtual Status STATUS { get; set; }
        public virtual DateTime START_DATE { get; set; }
        public virtual DateTime? END_DATE { get; set; }
        public virtual ISet<EMPLOYEE> EMPLOYEES { get; set; }
        public virtual GROUPS GROUP { get; set; }
        public virtual int VERSION { get; set; }
    }
}