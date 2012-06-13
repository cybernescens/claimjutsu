namespace HealthBus.CmsFss.Data
{
    public class CmsRvu2012a
    {
        public string Hcpcs { get; set; }
        public string Modifier { get; set; }
        public string StatusCode { get; set; }
        public decimal Work { get; set; }
        public decimal TransitionedNonFacilityPracticeExpense { get; set; }
		public bool TransitionedNonFacilityPracticeExpenseIndicator { get; set; }
		public decimal TransitionedFacilityPracticeExpense { get; set; }
		public bool TransitionedFacilityPracticeExpenseIndicator { get; set; }
		public decimal Malpractice { get; set; }
    }
}
