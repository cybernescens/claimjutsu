namespace HealthBus.CmsFss.Messages
{
    public class CmsFssResponse
    {
        public string Hcpcs { get; set; }
        public string Modifier { get; set; }
        public decimal Amount { get; set; }
    	public string Status { get; set; }
    }
}