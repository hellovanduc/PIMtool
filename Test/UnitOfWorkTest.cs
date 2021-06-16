using NUnit.Framework;
using Repositories;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using Unity;

namespace Test
{
    [TestFixture]
    public class UnitOfWorkTest
    {
        private IUnitOfWork unitOfWork;

        private IGenericRepository genericRepository;
        private IEmployeeRepository employeeRepository;

        [SetUp]
        public void SetUp()
        {
            //  Register components for dependency injection
            var container = new UnityContainer();

            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IGenericRepository, GenericRepository>();
            container.RegisterSingleton<IEmployeeRepository, EmployeeRepository>();

            unitOfWork = container.Resolve<IUnitOfWork>();
            genericRepository = container.Resolve<IGenericRepository>();
            employeeRepository = container.Resolve<IEmployeeRepository>();
        }

        [Test]
        public void TestSingleUnitOfWorkPattern__TwoFirstTransactionsSuccessLastTransactionFailed__AllTransactionShouldBeRollback()
        {
            //  Arrange
            EMPLOYEE e1 = new EMPLOYEE { FIRST_NAME = "Nguyen", LAST_NAME = "Duc", VISA = "PKH", BIRTH_DATE = new DateTime(2000, 3, 26) };
            EMPLOYEE e2 = new EMPLOYEE { FIRST_NAME = "Tran", LAST_NAME = "Dat", VISA = "HKT", BIRTH_DATE = new DateTime(2000, 3, 26) };
            EMPLOYEE e3 = new EMPLOYEE { FIRST_NAME = "Tran", LAST_NAME = "Dat", VISA = e2.VISA, BIRTH_DATE = new DateTime(2000, 3, 26) };

            //  Act
            using (unitOfWork.Start())
            {
                genericRepository.Save(e1);

                genericRepository.Save(e2);

                genericRepository.Save(e3);

                TestDelegate commit = () => unitOfWork.Commit();
                Assert.Throws<NHibernate.Exceptions.GenericADOException>(commit, "could not execute batch command.[SQL: SQL not available]");
            }

            //  Assert
            using (unitOfWork.Start())
            {
                var visas = new List<string>();
                visas.Add(e1.VISA);
                visas.Add(e2.VISA);
                Assert.AreEqual(0, employeeRepository.FindEmployeesByVisas(visas).Count);
            }
        }
    }
}