using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using HealthBus.CmsFss.Data;
using Raven.Client;
using Raven.Client.Document;

namespace HealthBus.ImportCmsRvu2012
{
	class Program
	{
		static void Main(string[] args)
		{
			var engine = new FileHelperEngine<Rvu2012Record>();
			var fsses = engine.ReadFile("Files\\PPRRVU12.csv");

			var store = new DocumentStore {Url = "http://localhost:8080"};
			store.Initialize();

			using (IDocumentSession session = store.OpenSession())
			{
				session.Advanced.MaxNumberOfRequestsPerSession = 999;

				var dels = new List<CmsRvu2012a>();
				do
				{
					dels = session
						.Query<CmsRvu2012a>()
						.Customize(x => x.WaitForNonStaleResults())
						.Take(1024)
						.ToList();

					dels.ForEach(session.Delete);
					session.SaveChanges();
				} while (dels.Count > 0);
			}

			using (IDocumentSession session = store.OpenSession())
			{
				session.Advanced.MaxNumberOfRequestsPerSession = 999;

				var finals = fsses.Select(
					rvu =>
					new CmsRvu2012a
				    {
				        Hcpcs = rvu.Hcpcs,
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
				foreach (var final in finals)
				{
					session.Store(final);
					i++;

					if (i%1024 == 0)
						session.SaveChanges();
				}

				session.SaveChanges();
			}
		}
	}
}