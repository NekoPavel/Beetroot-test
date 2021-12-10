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
                    string languageSearch = "SELECT * FROM languages WHERE employee_number=@employee_number";
                    using var con2 = new SqlConnection(cs);
                    con2.Open();
                    using var languageCommand = new SqlCommand(languageSearch, con2);
                    languageCommand.Parameters.Add("@employee_number", SqlDbType.VarChar, 255).Value = employeeReader.GetInt32(0);
                    languageCommand.Prepare();
                    using SqlDataReader languageReader = languageCommand.ExecuteReader();
                    List<Language> languages = new();
                    while (languageReader.Read())
                    {
                        languages.Add(new Language(languageReader.GetString(2), languageReader.GetInt32(3)));
                    }
                    employees.Add(new Employee(employeeReader.GetInt32(0), employeeReader.GetString(1), employeeReader.GetString(2), employeeReader.GetInt32(3), languages));
                }
                return employees;
            }
        }
        public static List<Employee> GetAll() => Employees;
        public static Employee? GetEmployeeByNumber(int employeeNumber)
        {
            Employee employee;
            var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using var con = new SqlConnection(cs);
            con.Open();
            string employeeSearch = "SELECT * FROM employees WHERE employee_number=@employee_number";
            using var employeeCommand = new SqlCommand(employeeSearch, con);
            employeeCommand.Parameters.Add("@employee_number", SqlDbType.VarChar, 255).Value = employeeNumber;

            using SqlDataReader employeeReader = employeeCommand.ExecuteReader();
            while (employeeReader.Read())
            {
                string languageSearch = "SELECT * FROM languages WHERE employee_number=@employee_number";
                using var con2 = new SqlConnection(cs);
                con2.Open();
                using var languageCommand = new SqlCommand(languageSearch, con2);
                languageCommand.Parameters.Add("@employee_number", SqlDbType.VarChar, 255).Value = employeeReader.GetInt32(0);
                languageCommand.Prepare();
                using SqlDataReader languageReader = languageCommand.ExecuteReader();
                List<Language> languages = new();
                while (languageReader.Read())
                {
                    languages.Add(new Language(languageReader.GetString(2), languageReader.GetInt32(3)));
                }
                employee = new Employee(employeeReader.GetInt32(0), employeeReader.GetString(1), employeeReader.GetString(2), employeeReader.GetInt32(3), languages);
                return employee;
            }
            return null;
        }
        public static void Add(Employee employee) // Add some kind of not empty check here I guess
        {
            var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using var con = new SqlConnection(cs);
            con.Open();
            var employeeQuery = "INSERT INTO employees(first_name, last_name, age) VALUES(@first_name, @last_name, @age)";
            using var employeeCmd = new SqlCommand(employeeQuery, con);

            employeeCmd.Parameters.Add("@first_name", SqlDbType.VarChar, 255).Value = employee.FirstName;
            employeeCmd.Parameters.Add("@last_name", SqlDbType.VarChar, 255).Value = employee.LastName;
            employeeCmd.Parameters.Add("@age", SqlDbType.Int).Value = employee.Age;
            employeeCmd.Prepare();

            employeeCmd.ExecuteNonQuery();

            string employeeSearch = "SELECT * FROM employees WHERE first_name=@first_name AND last_name=@last_name";
            using var employeeCommand = new SqlCommand(employeeSearch, con);
            employeeCommand.Parameters.Add("@first_name", SqlDbType.VarChar,255).Value = employee.FirstName;
            employeeCommand.Parameters.Add("@last_name", SqlDbType.VarChar,255).Value= employee.LastName;
            employeeCommand.Prepare();

            using SqlDataReader employeeReader = employeeCommand.ExecuteReader();
            while (employeeReader.Read())
            {
                employee.EmployeeNumber = employeeReader.GetInt32(0);
            }

            var languageQuery = "INSERT INTO languages(employee_number, language_name, proficiency_level) VALUES(@employee_number, @language_name, @proficiency_level)";
            foreach (Language language in employee.Languages)
            {
                using var con2 = new SqlConnection(cs);
                con2.Open();
                using var languageCmd = new SqlCommand(languageQuery, con2);
                languageCmd.Parameters.Add("@employee_number", SqlDbType.Int).Value = employee.EmployeeNumber;
                languageCmd.Parameters.Add("@language_name", SqlDbType.VarChar, 255).Value = language.LanguageName;
                languageCmd.Parameters.Add("@proficiency_level", SqlDbType.Int).Value = language.ProficiencyLevel;
                languageCmd.Prepare();

                languageCmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int employeeNumber)
        {
            var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using var con = new SqlConnection(cs);
            con.Open();
            var removeEmployeeQuery = "DELETE FROM employees WHERE employee_number=@employee_number";
            using var removeEmployeeCmd = new SqlCommand(removeEmployeeQuery, con);
            removeEmployeeCmd.Parameters.Add("@employee_number", SqlDbType.Int).Value = employeeNumber;
            removeEmployeeCmd.Prepare();
            removeEmployeeCmd.ExecuteNonQuery();
            var removeLanguagesQuery = "DELETE FROM languages WHERE employee_number=@employee_number";
            using var removeLanguagesCmd = new SqlCommand(removeLanguagesQuery, con);
            removeLanguagesCmd.Parameters.Add("@employee_number", SqlDbType.Int).Value = employeeNumber;
            removeLanguagesCmd.Prepare();
            removeLanguagesCmd.ExecuteNonQuery();
        }

        public static void Update(Employee employee) //find by id -> update all fields
        {
            var cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using var con = new SqlConnection(cs);
            con.Open();
            string employeeSearch = "UPDATE employees SET first_name=@first_name, last_name=@last_name, age=@age WHERE employee_number=@employee_number";
            using var employeeCommand = new SqlCommand(employeeSearch, con);
            employeeCommand.Parameters.Add("@employee_number", SqlDbType.VarChar, 255).Value = employee.EmployeeNumber;
            employeeCommand.Parameters.Add("@first_name", SqlDbType.VarChar, 255).Value = employee.FirstName;
            employeeCommand.Parameters.Add("@last_name", SqlDbType.VarChar, 255).Value = employee.LastName;
            employeeCommand.Parameters.Add("@age", SqlDbType.VarChar, 255).Value = employee.Age;
            employeeCommand.Prepare();
            employeeCommand.ExecuteNonQuery();

            var removeLanguagesQuery = "DELETE FROM languages WHERE employee_number=@employee_number";
            using var removeLanguagesCmd = new SqlCommand(removeLanguagesQuery, con);
            removeLanguagesCmd.Parameters.Add("@employee_number", SqlDbType.Int).Value = employee.EmployeeNumber;
            removeLanguagesCmd.Prepare();
            removeLanguagesCmd.ExecuteNonQuery();

            var languageQuery = "INSERT INTO languages(employee_number, language_name, proficiency_level) VALUES(@employee_number, @language_name, @proficiency_level)";
            foreach (Language language in employee.Languages)
            {
                using var languageCmd = new SqlCommand(languageQuery, con);
                languageCmd.Parameters.Add("@employee_number", SqlDbType.Int).Value = employee.EmployeeNumber;
                languageCmd.Parameters.Add("@language_name", SqlDbType.VarChar, 255).Value = language.LanguageName;
                languageCmd.Parameters.Add("@proficiency_level", SqlDbType.Int).Value = language.ProficiencyLevel;
                languageCmd.Prepare();

                languageCmd.ExecuteNonQuery();
            }
        }

        private static readonly List<Employee> employees = new();
    }
}
