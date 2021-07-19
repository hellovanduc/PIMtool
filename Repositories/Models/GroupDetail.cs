using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class GroupDetail
    {
        public long Id { get; set; }
        public DateTime EstablishDate { get; set; }
        public int NumberOfProjectHasDone { get; set; }
    }
}
