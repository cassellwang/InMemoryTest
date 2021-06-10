using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyInMemoryTest.Production;
using MyInMemoryTest.Production.Entities;
using NUnit.Framework;

namespace MyInMemoryTest
{
    public class EmployeeRepositoryTests
    {
        private IEmployeeRepository _employeeRepository;
        private EmployeeContext _testDBContext;

        [SetUp]
        public void Setup()
        {
            this._employeeRepository = this.GetInMemoryRepository();
        }

        [Test]
        public void add_a_employee()
        {
            var employeeDto = new EmployeeDto() { Department = DepartmentEnum.火箭隊與她快樂夥伴, FirstName = "Elena", LastName = "Wang" };
            _employeeRepository.AddAsync(employeeDto);

            _testDBContext.Employees.Where(x => x.Id == 1).First().Name.Should().Be("Elena Wang");
        }

        [Test]
        public async Task find_海外企業開發組()
        {
            var employeeDto1 = new EmployeeDto() { Department = DepartmentEnum.海外企業開發組, FirstName = "Cassell", LastName = "Wang" };
            var employeeDto2 = new EmployeeDto() { Department = DepartmentEnum.海外企業開發組, FirstName = "James", LastName = "Wang" };
            var employeeDto3 = new EmployeeDto() { Department = DepartmentEnum.火箭隊與她快樂夥伴, FirstName = "Elena", LastName = "Wang" };
            await _employeeRepository.AddAsync(employeeDto1);
            await _employeeRepository.AddAsync(employeeDto2);
            await _employeeRepository.AddAsync(employeeDto3);

            var employees = await _employeeRepository.FindByDepartmentId(DepartmentEnum.海外企業開發組);
            employees.Count().Should().Be(2);
        }

        private IEmployeeRepository GetInMemoryRepository()
        {
            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: "MockDB")
                .Options;

            _testDBContext = new EmployeeContext(options);
            var repository = new EmployeeRepository(_testDBContext);

            return repository;
        }
    }
}