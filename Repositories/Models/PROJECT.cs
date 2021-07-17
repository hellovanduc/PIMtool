
using Repositories.Enums;
using System;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class Project
    {
        public long ID { get; set; }
        public int PROJECT_NUMBER { get; set; }
        public string NAME { get; set; }
        public string CUSTOMER { get; set; }
        public Status STATUS { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public virtual ISet<Employee> EMPLOYEES { get; set; }
        public virtual Group GROUP { get; set; }
        public int VERSION { get; set; }
    }
}