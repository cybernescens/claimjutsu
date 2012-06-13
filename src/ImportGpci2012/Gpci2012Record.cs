using FileHelpers;

namespace HealthBus.ImportGpci2012
{
    [DelimitedRecord(",")]
	[IgnoreFirst(3)]
	[IgnoreLast(16)]
    public class Gpci2012Record
    {
        public string Contractor;
        public string Locality;
        [FieldQuoted(QuoteMode.OptionalForRead)]
        public string LocalityName;
        public decimal Work;
        public decimal PracticeExpense;
        public decimal MalpracticeExpense;
    }
}