namespace CRMYourBankers.Models
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Validate()
        {
            return true;
        }
    }
}
