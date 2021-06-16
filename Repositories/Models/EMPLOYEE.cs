using System;

namespace Repositories.Models
{
    public class EMPLOYEE
    {
        public virtual long ID { get; set; }
        public virtual string VISA { get; set; }
        public virtual string FIRST_NAME { get; set; }
        public virtual string LAST_NAME { get; set; }
        public virtual DateTime BIRTH_DATE { get; set; }
        public virtual int VERSION { get; set; }
    }
}