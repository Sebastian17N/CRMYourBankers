namespace CRMYourBankers.Models
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }        
        public long? PersonalId { get; set; }
        public int Id { get; set; }

        public string FullName => $"{FirstName} {LastName} {Email}";


        public bool Validate()
            //validate sprawdza poprawność 
        {
            return
            FirstName != "" &&
            LastName != "" &&
            PhoneNumber != null &&
            Email != "" &&
            PersonalId != null;
        }       
    }
}
