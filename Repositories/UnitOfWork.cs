using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using Repositories.Interfaces;
using System;
using System.Reflection;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory sessionFactory;
        private ITransaction transaction;
        private ISession session;

        public ISession Session { get => session; }

        public UnitOfWork()
        {
            //  Configure mapping
            var config = new Configuration().Configure();
            var mapping = GetMappings();
            config.AddDeserializedMapping(mapping, null);

            //  Create session factory
            sessionFactory = config.BuildSessionFactory();
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }


        public IUnitOfWork Start()
        {
            if (session == null || session.IsOpen == false)
            {
                session = sessionFactory.OpenSession();
            }
            if (transaction == null || transaction.IsActive == false)
            {
                transaction = session.BeginTransaction();
            }
            return this;
        }
        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            if (transaction != null && transaction.IsActive == true)
            {
                transaction.Dispose();
            }
            if (session != null && session.IsOpen)
            {
                session.Dispose();
            }
        }
    }
}