namespace API.Models
{
    public class Language
    {
        private string languageName;
        private int proficiencyLevel;

        public Language(string languageName, int proficiencyLevel)
        {
            this.languageName = languageName;
            this.proficiencyLevel = proficiencyLevel;
        }
    }
}