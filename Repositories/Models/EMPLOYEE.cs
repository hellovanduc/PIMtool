using System;

namespace Repositories.Models
{
    public class Employee
    {
        public long ID { get; set; }
        public string VISA { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public DateTime BIRTH_DATE { get; set; }
        public int VERSION { get; set; }
    }
}