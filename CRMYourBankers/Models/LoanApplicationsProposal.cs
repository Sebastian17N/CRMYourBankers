namespace CRMYourBankers.Models
{
	public class LoanApplicationsProposal
	{
		public int ClientId { get; set; }
		public int ProposalIndex { get; set; }
		public int? BankId { get; set; }

		public Client Client { get; set; }
		public Bank Bank { get; set; }
	}
}
