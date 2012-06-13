using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using HealthBus.CmsFss.Data;
using Raven.Client.Document;

namespace HealthBus.ImportCmsRvu2011
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new FileHelperEngine<Rvu2011Record>();
            var fsses = engine.ReadFile("Files\\PPRRVU11_OCT_082511_v2.csv");

            var store = new DocumentStore
                        	{
                        		Url = "https://1.ravenhq.com/databases/AppHarbor_2f837d84-a32e-41ea-a332-67a4dfceb11c",
								ApiKey = "689ae9c4-9315-4501-88de-32f965f3ff0f"
                        	};
            store.Initialize();

			using(var session = store.OpenSession())
			{
				session.Advanced.MaxNumberOfRequestsPerSession = 999;

				var dels = new List<CmsRvu2011d>();
				do
				{
					dels = session
						.Query<CmsRvu2011d>()
						.Customize(x => x.WaitForNonStaleResults())
						.Take(1024)
						.ToList();

					dels.ForEach(session.Delete);
					session.SaveChanges();
				} while (dels.Count > 0);
			}

            using(var session = store.OpenSession())
            {
				session.Advanced.MaxNumberOfRequestsPerSession = 999;

                var finals = fsses.Select(rvu => 
                    new CmsRvu2011d
                        {
                          Hcpcs= rvu.Hcpcs,
                          Modifier = string.IsNullOrEmpty(rvu.Modifier) ? null : rvu.Modifier,
                          Work = rvu.WorkRvu ?? 0,
                          TransitionedFacilityPracticeExpense = rvu.TransitionedFacilityPracticeExpenseRvu ?? 0,
						  TransitionedFacilityPracticeExpenseIndicator = rvu.TransitionedFacilityNaIndicator == "NA" ? true : false,
						  TransitionedNonFacilityPracticeExpense = rvu.TransitionedNonFacilityPractiveExpenseRvu ?? 0,
						  TransitionedNonFacilityPracticeExpenseIndicator = rvu.TransitionedNonFacilityNaIndicator == "NA" ? true : false,
						  Malpractice = rvu.MalpracticeRvu ?? 0
                        })
                    .ToList();

            	int i = 0;
				foreach(var final in finals)
				{
					session.Store(final);
					i++;

					if(i % 1024 == 0)
						session.SaveChanges();
				}

				session.SaveChanges();
            }
        }
    }
}
