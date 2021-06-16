using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;

namespace Repositories.Mapping
{
    public class EmployeeMap : ClassMapping<EMPLOYEE>
    {
        public EmployeeMap()
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
            Property(x => x.VISA, m =>
            {
                m.Unique(true);
                m.Length(3);
                m.NotNullable(true);
                m.Column(c => c.SqlType("CHAR(3)"));
            });
            Property(x => x.FIRST_NAME, m =>
            {
                m.NotNullable(true);
                m.Length(50);
                m.Column(c => c.SqlType("VARCHAR(50)"));
            });
            Property(x => x.LAST_NAME, m =>
            {
                m.NotNullable(true);
                m.Length(50);
                m.Column(c => c.SqlType("VARCHAR(50)"));
            });
            Property(x => x.BIRTH_DATE, m =>
            {
                m.NotNullable(true);
                m.Column(c => c.SqlType("DATE"));
            });
        }
    }
}