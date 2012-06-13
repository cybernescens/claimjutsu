using FileHelpers;

namespace HealthBus.ImportGpci2011
{
    [DelimitedRecord(",")]
    [IgnoreFirst(3)]
    public class Gpci2011Record
    {
        public string Contractor;
        public string Locality;
        [FieldQuoted(QuoteMode.OptionalForRead)]
        public string LocalityName;
        public decimal Work2010;
        public decimal PracticeExpense2010;
        public decimal MalpracticeExpense2010;
        public decimal Work;
        public decimal PracticeExpense;
        public decimal MalpracticeExpense;
    }
}