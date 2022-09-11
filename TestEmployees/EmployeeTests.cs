using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EmployeeHeirachy;


namespace TestEmployees
{
    [TestClass]
    public class EmployeeTests
    {

        // The csv is valid
        [TestMethod]
        public void InvalidCsvThrowsException()
        {
            Assert.ThrowsException<Exception>(() => new Employees(""));
        }

        // The salaries in the CSV are valid integer numbers
        [TestMethod]
        public void InvalidSalaryThrowsException()
        {
            Assert.ThrowsException<Exception>(() => new Employees(
                "Employee4, Employee2, 500\n" +
                "Employee3, Employee1, hundred\n" + //error
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee2, Employee1, 500"
                ));
        }

        // One employee does not report to more than one manager.
        [TestMethod]
        public void manyManagersOneEmployeeThrowsException()
        {
            Assert.ThrowsException<Exception>(() => new Employees(
                "Employee4, Employee2, 500\n" +
                "Employee4, Employee1, 800\n" + //error
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee2, Employee1, 500"
                ));
        }

        // There is only one CEO, i.e. only one employee with no manager
        [TestMethod]
        public void moreThanOneCEOThrowsException()
        {
            Assert.ThrowsException<Exception>(() => new Employees(
                "Employee4, Employee2, 500\n" +
                "Employee4, Employee1, 800\n" +
                "Employee6,, 1000\n" + // error
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee2, Employee1, 500"
                ));
        }

        // There is no circular reference, i.e. a first employee reporting to a second employee that is also under the first employee.
        
        [TestMethod]
        public void circularReferenceThrowsException()
        {
            Assert.ThrowsException<Exception>(() => new Employees(
                "Employee4, Employee2, 500\n" +
                "Employee4, Employee1, 800\n" + // error
                "Employee6,, 1000\n" +
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee2, Employee1, 500"
                ));
        }

        // There is no manager that is not an employee, i.e. all managers are also listed in the employee column.
        [TestMethod]
        public void managerNotEmployeeThrowsException()
        {
            Assert.ThrowsException<Exception>(() => new Employees(
                "Employee4, Employee2, 500\n" +
                "Employee3, Employee1, 800\n" +
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee7, Employee8, 500\n" + // error
                "Employee2, Employee1, 500"
                ));
        }

        // Salary budget calculation by manager
        [TestMethod]
        public void wrongSalaryBudgetThrowsException()
        {
            Employees employees = new Employees(
                "Employee4, Employee2, 500\n" +
                "Employee3, Employee1, 800\n" +
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee2, Employee1, 500"
                );
            Assert.AreEqual(2800, employees.salaryBudgetByManager("Employee1"));
        }
    }
}