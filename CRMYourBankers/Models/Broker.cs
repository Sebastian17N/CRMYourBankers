using System.Collections.Generic;

namespace CRMYourBankers.Models
{
	public class Broker
    {
        public string Name { get; set; }
        public int Id { get; set; }
        
        public List<Client> Clients { get; set; }        
    }
}
