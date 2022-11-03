using System;

namespace CRMYourBankers.Models
{
    public class ClientTask
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Done { get; set; }
        public int ClientId { get; set; } 
        public Client Client { get; set; }
        public DateTime? TaskAddedDate { get; set; }
        public DateTime? TaskDate { get; set; }
    }
}
