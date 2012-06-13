using System;
using FileHelpers;

namespace HealthBus.ImportCmsRvu2011
{
    [DelimitedRecord(",")]
	[IgnoreFirst(10)]
	[IgnoreLast(1)]
    public class Rvu2011Record
    {
        public string Hcpcs;


        public string Modifier;

        [FieldQuoted(QuoteMode.OptionalForRead)]
        public string Description;

        /// <summary>
        /// A =   Active Code.  These codes are paid separately under the physician fee schedule, if covered.  
        ///     There will be RVUs for codes with this status.  The presence of an "A" indicator does not mean that 
        ///     Medicare has made a national coverage determination regarding the service; carriers remain responsible 
        ///     for coverage decisions in the absence of a national Medicare policy.
        /// B =  	Bundled Code.  Payment for covered services are always bundled into payment for other services 
        ///     not specified.  If RVUs are shown, they are not used for Medicare payment.  If these services are covered, 
        ///     payment for them is subsumed by the payment for the services to which they are incident. 
        ///     (An example is a telephone call from a hospital nurse regarding care of a patient).
        /// C =	Carriers price the code.  Carriers will establish RVUs and payment amounts for these services, 
        ///     generally on an individual case basis following review of documentation such as an operative report.
        /// D =	Deleted Codes.  These codes are deleted effective with the beginning of the applicable year.    
        ///     These codes will not appear on the 2006 file as the grace period for deleted codes is no longer applicable.    
        /// E =	Excluded from Physician Fee Schedule by regulation.  These codes are for items and/or services that CMS chose 
        ///     to exclude from the fee schedule payment by regulation.  No RVUs are shown, and no payment may be made under 
        ///     the fee schedule for these codes.  Payment for them, when covered, generally continues under reasonable charge procedures. 
        /// F =  	Deleted/Discontinued Codes.  (Code not subject to a 90 day grace period). These codes will not appear 
        ///     on the 2006 file as the grace period for deleted codes is no longer applicable.    
        /// G =	Not valid for Medicare purposes.  Medicare uses another code for reporting of, and payment for, these services.  
        ///     (Code subject to a 90 day grace period.)  These codes will not appear on the 2006 file as the grace period for 
        ///     deleted codes is no longer applicable.    
        /// H =	Deleted Modifier.  This code had an associated TC and/or 26 modifier in the previous year.  
        ///     For the current year, the TC or 26 component shown for the code has been deleted, and the deleted 
        ///     component is shown with a status code of "H".   These codes will not appear on the 2006 file as the grace 
        ///     period for deleted codes is no longer applicable.     
        /// I = 	Not valid for Medicare purposes.  Medicare uses another code for reporting of, and payment for, 
        ///     these services.  (Code NOT subject to a 90 day grace period.)
        /// J  =   Anesthesia Services.   There are no RVUs and no payment amounts for these codes.    
        ///     The intent of this value is to facilitate the identification of anesthesia services.  
        /// M =   Measurement codes.   Used for reporting purposes only. 
        /// N =	Non-covered Services.  These services are not covered by Medicare.
        /// P =	Bundled/Excluded Codes.  There are no RVUs and no payment amounts for these services.  
        ///     No separate payment should be made for them under the fee schedule.
        ///     --If the item or service is covered as incident to a physician service and is provided on 
        ///     the same day as a physician service, payment for it is bundled into the payment for the physician 
        ///     service to which it is incident.  (An example is an elastic bandage furnished by a 
        ///     physician incident to physician service.)
        ///     --If the item or service is covered as other than incident to a physician service, 
        ///     it is excluded from the fee schedule (i.e., colostomy supplies) and should be paid 
        ///     under the other payment provision of the Act.
        /// R =	Restricted Coverage.  Special coverage instructions apply.  If covered, the service is carrier priced.  
        ///     (NOTE:  The majority of codes to which this indicator will be assigned are the alpha-numeric dental 
        ///     codes, which begin with "D".  We are assigning the indicator to a limited number of CPT codes which 
        ///     represent services that are covered only in unusual circumstances.) 
        /// T =	Injections.  There are RVUS and payment amounts for these services, but they are only paid if there 
        ///     are no other services payable under the physician fee schedule billed on the same date by the same provider.  
        ///     If any other services payable under the physician fee schedule are billed on the same date by the same provider, 
        ///     these services are bundled into the physician services for which payment is made.  
        ///     (NOTE:  This is a change from the previous definition, which states that injection services are bundled into any other services billed on the same date.)
        /// X =	Statutory Exclusion.  These codes represent an item or service that is not in the statutory definition of 
        ///     "physician services" for fee schedule payment purposes.  No RVUS or payment amounts are shown for these codes, 
        ///     and no payment may be made under the physician fee schedule.  
        ///     (Examples are ambulance services and clinical diagnostic laboratory services.)  
        /// </summary>
        public string StatusCode;

        //? 

        [FieldConverter(typeof(RemovePlusConverter))]
        public decimal? Payment;

        public decimal? WorkRvu;
        public decimal? TransitionedNonFacilityPractiveExpenseRvu;
        public string TransitionedNonFacilityNaIndicator;
        public decimal? FullyImplementedNonFacilityPracticeExpenseRvu;
        public string FullyImplementNonFacilityNaIndicator;
        public decimal? TransitionedFacilityPracticeExpenseRvu;
        public string TransitionedFacilityNaIndicator;
        public decimal? FullyImplementedFacilityPracticeExpenseRvu;
        public string FullyImplementedFacilityNaIndicator;
        public decimal? MalpracticeRvu;
        public decimal? TotalTransitionedNonFacilityRvu;
        //public string Filler;
        public decimal? TotallyFullyImplementedNonFaciltiyRvus;
        public decimal? TotalTransitionedFacilityRvu;
        public decimal? TotalFullyImplementedFacilityRvus;
        public string PcTcIndicator;
        public string GlobalSurgery;
        public decimal? PreoperativePercentage;
        public decimal? IntraoperativePercentage;
        public decimal? PostoperativePercentage;
        public string MultipleProcedure;
        public string BilateralSurgery;
        public string AssistAtSurgery;
        public string CoSurgeons;
        public string TeamSurgery;
        //public string Filler2;
        //public string Filler3;
        public string EndoscopicBaseCode;
        public decimal? ConversionFactory;
        public string PhysicianSupervisionOfDiagnosticProcedures;
        public string CalculatorFlag;
        public string DiagnosticImagingFamilyIndicator;
        public decimal? NonFacilityPracticeExpenseUsedForOppsPaymentAmount;
        public decimal? FacilityPracticeExpenseUsedForOppsPaymentAmount;
        public decimal? MalpracticeUsedForOppsPaymentAmount;


    }

    public class RemovePlusConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (string.IsNullOrEmpty(@from))
                return null;

            var temp = @from.Replace("+", "");

            if (string.IsNullOrEmpty(temp))
                return null;

            return Convert.ToDecimal(temp);
        }
    }
}