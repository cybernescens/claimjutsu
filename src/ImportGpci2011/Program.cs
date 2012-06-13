using System;
using System.Collections.Generic;
using FileHelpers;
using System.Linq;
using HealthBus.CmsFss.Data;
using Raven.Client.Document;

namespace HealthBus.ImportGpci2011
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new FileHelperEngine<Gpci2011Record>();
            var gpcis = engine.ReadFile("Files\\GPCI1.csv");

            var engine2 = new FileHelperEngine<ZipToCarrier2011Record>();
            var zips = engine2.ReadFile("Files\\ZIP5_FINAL11_DEC.txt");

            Func<string, string, string> toKey =
                (c, l) => string.Format("{0}{1}", c, l);

            var gpciDic = gpcis
                .ToDictionary(x => toKey(x.Contractor, x.Locality));

            var store = new DocumentStore {Url = "http://localhost:8080"};
            store.Initialize();

			using (var session = store.OpenSession())
			{
				session.Advanced.MaxNumberOfRequestsPerSession = 999;

				var dels = new List<CmsGpciZip2011d>();
				do
				{
					dels = session
						.Query<CmsGpciZip2011d>()
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

            	var finals = zips.Select(x =>
					 {
						 var key = toKey(x.Carrier, x.PricingLocality);
						 var gpci = gpciDic[key];

						 return new CmsGpciZip2011d
							 {
								 Zip = x.ZipCode,
								 Carrier = gpci.Contractor,
								 Locality = gpci.Locality,
								 Malpractice = gpci.MalpracticeExpense,
								 PracticeExpense = gpci.PracticeExpense,
								 Work = gpci.Work
							 };
					 })
					 .ToList();

				int i = 0;
				foreach (var final in finals)
				{
					session.Store(final);
					i++;

					if (i % 1024 == 0)
						session.SaveChanges();
				}

				session.SaveChanges();
            }
        }
    }
}
