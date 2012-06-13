using FileHelpers;

namespace HealthBus.ImportGpci2011
{
    /*
     * Field Name             Beg. Position     End Position    Length            Comments

State		   	                1                 2             2
Zip Code		                3   	          7             5
Carrier			                8                 12            5
Pricing Locality                13                14            2
Rural Indicator       	        15	     	      15            1      Effective 1/1/2007
								                                            Blank=urban, R=rural, B=super rural
Bene Lab CB Locality	        16                17            2      Lab competitive bid locality
									                                    Z1 = CBA 1
									                                    Z2 = CBA 2
									                                    Z9 = Not a dem  onstration locality
Rural Indicator2 	            18		          18            1      What was effective 12/1/2006
Filler			                19                20            2
Plus Four Flag 		            21                21            1      0 = no +4 extension, 1 = +4 extension
Filler			                22                75            54
Year/Quarter		            76                80            5		YYYYQ
     */
    [FixedLengthRecord()]
    public class ZipToCarrier2011Record
    {
        [FieldFixedLength(2)] 
        public string State;
        [FieldFixedLength(5)]
        public string ZipCode;
        [FieldFixedLength(5)]
        public string Carrier;
        [FieldFixedLength(2)]
        public string PricingLocality;
        [FieldFixedLength(1)]
        public string RuralIndicator;
        [FieldFixedLength(2)]
        public string BeneLabCbLocality;
        [FieldFixedLength(1)]
        public string RuralIndicator2;
        [FieldFixedLength(2)]
        public string Filler;
        [FieldFixedLength(1)]
        public string PlusFourFlag;
        [FieldFixedLength(54)]
        public string Filler2;
        [FieldFixedLength(5)]
        public string YearQuarter;
    }
}