namespace Repositories.Models
{
    public class GROUPS
    {
        public virtual long ID { get; set; }
        public virtual string NAME { get; set; }
        public virtual EMPLOYEE GROUP_LEADER { get; set; }
        public virtual int VERSION { get; set; }
    }
}