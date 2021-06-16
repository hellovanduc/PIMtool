using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;

namespace Repositories.Mapping
{
    public class ProjectMap : ClassMapping<PROJECT>
    {
        public ProjectMap()
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
            Property(x => x.PROJECT_NUMBER, m =>
            {
                m.Unique(true);
                m.NotNullable(true);
                m.Length(4);
                m.Column(c => c.SqlType("NUMERIC(4,0)"));
            });
            Property(x => x.NAME, m =>
            {
                m.NotNullable(true);
                m.Length(50);
                m.Column(c => c.SqlType("VARCHAR(50)"));
            });
            Property(x => x.CUSTOMER, m =>
            {
                m.NotNullable(true);
                m.Length(50);
                m.Column(c => c.SqlType("VARCHAR(50)"));
            });
            Property(x => x.STATUS, m =>
            {
                m.NotNullable(true);
            });
            Property(x => x.START_DATE, m =>
            {
                m.Column(c => c.SqlType("DATE"));
                m.NotNullable(true);
            });
            Property(x => x.END_DATE, m =>
            {
                m.Column(c => c.SqlType("DATE"));
            });

            ManyToOne(x => x.GROUP, m =>
            {
                m.NotNullable(true);
                m.Column("GROUP_ID");
                m.Column(c => c.SqlType("NUMERIC(19,0)"));
            });

            Set(x => x.EMPLOYEES, m =>
            {
                m.Key(k =>
                {
                    k.Column(c =>
                    {
                        c.Name("EMPLOYEE_ID");
                        c.SqlType("NUMERIC(19,0)");
                    });
                });
                m.Table("PROJECT_EMPLOYEE");
            }, r => r.ManyToMany(m => m.Column(c =>
            {
                c.Name("PROJECT_ID");
                c.SqlType("NUMERIC(19,0)");
            })));
        }
    }
}