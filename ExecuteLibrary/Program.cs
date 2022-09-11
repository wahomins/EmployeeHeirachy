using System;
using EmployeeHeirachy;
namespace ExecuteEmployees
{
    class Program
    {

        static void Main(string[] args)
        {
            Employees employees = new Employees (
                "Employee4, Employee2, 500\n" +
                "Employee3, Employee1, 800\n" +
                "Employee1,, 1000\n" +
                "Employee5, Employee1, 500\n" +
                "Employee2, Employee1, 500"
                );

            long salary = employees.salaryBudgetByManager("Employee2");

            Console.WriteLine("Total Salary budget for manager is:- " + salary);
        }
    }
}
