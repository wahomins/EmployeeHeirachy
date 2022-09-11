using System.Collections;
using System.Data.Common;

namespace EmployeeHeirachy
{
    public class Employees
    {

        ArrayList employeeList;

        public Employees(string csv)
        {
            try
            {
               employeeList = parseCSV(csv);
                validateCSVData(employeeList);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /* convert csv to a Multi-dimensional arrayList */
        public ArrayList parseCSV (string csv)
        {
            ArrayList employees = new ArrayList();
            if (string.IsNullOrEmpty(csv) || !(csv is string))
            {
                throw new Exception("CSV INVALID, Nothing to process");

            }

            // convert csv rows to an array

           string[] datarows = csv.Split(
           Environment.NewLine.ToCharArray()
           );

            // let's make sure our csv data is as expected

            foreach (string row in datarows)
            {
                string[] data = row.Split(',');
                ArrayList expectedData = new ArrayList();

                foreach (string cell in data)
                {
                    expectedData.Add(cell);
                }
                if (expectedData.Count != 3)
                {
                    throw new Exception("Input csv data is wrongly formatted");
                }
                employees.Add(expectedData);
            }

            return employees;
        }

        // let's validate the csv entries 
        public void validateCSVData(ArrayList employees)
        {
            ArrayList validatedEmployees = new ArrayList(); //List of ValidatedEmployees
            ArrayList validatedManagers = new ArrayList(); //List of validatedManagers
            ArrayList ceos = new ArrayList();
            ArrayList otherEmployees = new ArrayList(); // any other junior employee

            foreach (ArrayList employee in employees)
            {

                string? employeeName = employee[0] as string ?? "";
                string? managerName = employee[1] as string ?? "";
                string? salary = employee[2] as string;

                // Check that salary is a valid integer
                int number;
                if (!(Int32.TryParse(salary, out number)))
                {
                    throw new Exception("INVALID SALARY VALUE, Salary should be a number");
                }

                // check that One employee does not report to more than one manager
                if (validatedEmployees.Contains(employeeName.Trim()))
                {
                    throw new Exception("DUPLICATE EMPLOYEE, Employee should report to one manager");
                }

                validatedEmployees.Add(employeeName.Trim());

                if (!string.IsNullOrEmpty(managerName.Trim()))
                {
                    validatedManagers.Add(managerName.Trim());
                }
                else
                {
                    ceos.Add(employeeName.Trim());
                }

            }

            // check that There is only one CEO, i.e. only one employee with no manager
            int managersDiff = employees.Count - validatedManagers.Count;
            if (managersDiff != 1)
            {
                throw new Exception("CEO NOT FOUND, All employees have a manager");
            }

            // check that There is no manager that is not an employee, i.e. all managers are also listed in the employee column
            foreach (string manager in validatedManagers)
            {
                if (!validatedEmployees.Contains(manager.Trim()))
                {
                    throw new Exception("WRONG MANAGER DATA, There are some mangers who are not listed as employees");
                }
            }


            // Include other employees
            foreach (string employee in validatedEmployees)
            {
                if (!validatedManagers.Contains(employee) && !ceos.Contains(employee))
                {
                    otherEmployees.Add(employee.Trim());
                }
            }

            // check that There is no circular reference, i.e. a first employee reporting to a second employee that is also under the first employee
            for (var i = 0; i < employees.Count; i++)
            {
                var allEmployees = employees[i] as ArrayList;
                string? employeeManager = allEmployees[1] as string ?? "";
                int index = validatedEmployees.IndexOf(employeeManager);

                if (index != -1)
                {
                    var managerData = employees[index] as ArrayList;
                    var lineManager = managerData[1] as string;

                    if ((!ceos.Contains(lineManager.Trim()) && validatedManagers.Contains(lineManager.Trim()))
                        || otherEmployees.Contains(lineManager.Trim()))
                    {
                        throw new Exception("CIRCULAR REFERENCE, there's a first employee reporting to a second employee that is also under the first employee");
                    }
                }
            }
        }

        // Return salary budget for a specific manager

        public long salaryBudgetByManager(string manager)
        {
            long totalSalary = 0;
            foreach (ArrayList employee in employeeList)
            {
                var managerName = employee[1] as string;
                var employeeSalary = employee[2] as string;
                var employeName = employee[0] as string;
                if (managerName.Trim() == manager.Trim() || employeName.Trim() == manager.Trim())
                {
                    totalSalary += Convert.ToInt32(employeeSalary);
                }
            }
            return totalSalary;
        }
    }
}