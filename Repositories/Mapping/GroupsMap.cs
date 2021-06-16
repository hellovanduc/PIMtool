using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;

namespace Repositories.Mapping
{
    public class GroupsMap : ClassMapping<GROUPS>
    {
        public GroupsMap()
        {
            Id(x => x.ID, m =>
            {
                m.Generator(Generators.Sequence);
                m.Column(c => c.SqlType("NUMERIC(19,0)"));
            });
            Version(x => x.VERSION, m =>
            {
                m.Column(c => c.SqlType("NUMERIC(10,0)"));
            });
            Property(x => x.NAME, m =>
            {
                m.Unique(true);
                m.Length(50);
                m.NotNullable(true);
                m.Column(c => c.SqlType("VARCHAR(50)"));
            });
            ManyToOne(x => x.GROUP_LEADER, m =>
            {
                m.Unique(true);
                m.NotNullable(true);
                m.Column(c => c.SqlType("NUMERIC(19,0)"));
            });
        }
    }
}