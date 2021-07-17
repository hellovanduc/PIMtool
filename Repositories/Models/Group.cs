using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Group
    {
        public long ID { get; set; }
        public string NAME { get; set; }
        [Index(IsUnique = true)]
        public virtual Employee GROUP_LEADER { get; set; }
        public int VERSION { get; set; }
    }
}