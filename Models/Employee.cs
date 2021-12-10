namespace API.Models
{
    public class Employee
    {
        private int employeeNumber;
        private string firstName;
        private string lastName;
        private int age;
        private List<Language> languages;

        public Employee(int employeeNumber, string firstName, string lastName, int age, List<Language> languages)
        {
            this.employeeNumber = employeeNumber;
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.languages = languages;
        }
        public string FirstName() { return firstName; }
        public string LastName() { return lastName; }
        public int Age() { return age; }
    }
}