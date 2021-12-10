using API.Models;
using System.Data.SqlClient;
using System.Data;

namespace API.Services
{
    public static class EmployeeService
    {
        static EmployeeService()
        {

        }
        static List<Employee> Employees
        {
            get
            {
                var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using var con = new SqlConnection(cs);
                con.Open();
                string employeeSearch = "SELECT * FROM employees";
                using var employeeCommand = new SqlCommand(employeeSearch, con);
                using SqlDataReader employeeReader = employeeCommand.ExecuteReader();
                while (employeeReader.Read())
                {
                    string languageSearch = $"SELECT * FROM languages WHERE employee_number={employeeReader.GetInt32(0)}";
                    using var languageCommand = new SqlCommand(languageSearch, con);
                    using SqlDataReader languageReader = languageCommand.ExecuteReader();
                    List<Language> languages = new();
                    while (languageReader.Read())
                    {
                        languages.Add(new Language(languageReader.GetString(1), languageReader.GetInt32(2)));
                    }
                    employees.Add(new Employee(employeeReader.GetInt32(0), employeeReader.GetString(1), employeeReader.GetString(2), employeeReader.GetInt32(3), languages));
                }
                return employees;
            }
        }
        public static Employee? GetEmployeeByNumber(int employeeNumber)
        {
            Employee employee;
            var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using var con = new SqlConnection(cs);
            con.Open();
            string employeeSearch = $"SELECT * FROM employees WHERE employee_number={employeeNumber}";
            using var employeeCommand = new SqlCommand(employeeSearch, con);
            using SqlDataReader employeeReader = employeeCommand.ExecuteReader();
            while (employeeReader.Read())
            {
                string languageSearch = $"SELECT * FROM languages WHERE employee_number={employeeReader.GetInt32(0)}";
                using var languageCommand = new SqlCommand(languageSearch, con);
                using SqlDataReader languageReader = languageCommand.ExecuteReader();
                List<Language> languages = new();
                while (languageReader.Read())
                {
                    languages.Add(new Language(languageReader.GetString(1), languageReader.GetInt32(2)));
                }
                employee = new Employee(employeeReader.GetInt32(0), employeeReader.GetString(1), employeeReader.GetString(2), employeeReader.GetInt32(3), languages);
                return employee;
            }
            return null;
        }
        public static void Add(Employee employee) // Add some kind of not empty check here i guess
        {
            var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using var con = new SqlConnection(cs);
            con.Open();
            var query = "INSERT INTO employees(first_name, last_name, age) VALUES(@first_name, @last_name, @age)";
            using var cmd = new SqlCommand(query, con);

            cmd.Parameters.Add("@first_name", SqlDbType.VarChar, 255).Value = employee.FirstName();
            cmd.Parameters.Add("@last_name", SqlDbType.VarChar, 255).Value = employee.LastName();
            cmd.Parameters.Add("@age", SqlDbType.Int).Value = employee.Age();
            cmd.Prepare();

            cmd.ExecuteNonQuery();

        }

        private static readonly List<Employee> employees = new();
    }
}
