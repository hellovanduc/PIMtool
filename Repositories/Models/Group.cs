namespace Repositories.Models
{
    public class Group
    {
        public long ID { get; set; }
        public string NAME { get; set; }
        public virtual Employee GROUP_LEADER { get; set; }
        public int VERSION { get; set; }
    }
}