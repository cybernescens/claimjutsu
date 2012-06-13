using System;
using System.Linq;
using System.ServiceModel.Activation;
using HealthBus.CmsFss.Data;
using HealthBus.CmsFss.Messages;
using HealthBus.CmsFss2011;
using Raven.Client;

namespace HealthBus.CmsFss
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class CmsFssFacility2012a : ICmsFssFacility2012a
	{
		public IDocumentSession Session { get; set; }

		public CmsFssResponse Get(string zip, string hcpcs)
		{
			return GetWithMod(zip, hcpcs, null);
		}

		public CmsFssResponse GetWithMod(string zip, string hcpcs, string modifier)
		{
			if (modifier == "00" || modifier == string.Empty)
				modifier = null;

			var rvus = Session.Query<CmsRvu2012a>()
				.Where(cms => cms.Hcpcs == hcpcs && cms.Modifier == modifier)
				.Take(2)
				.ToList();

			var gpcis = Session.Query<CmsGpciZip2012a>()
				.Where(gpc => gpc.Zip == zip)
				.Take(2)
				.ToList();

			//should probably be a 404
			if (rvus.Count < 1 || gpcis.Count < 1)
				return new CmsFssResponse { Amount = 0.0000m, Hcpcs = hcpcs, Modifier = modifier, Status = "NOT FOUND" };

			var rvu = rvus[0];
			var gpci = gpcis[0];

			if (rvu.TransitionedFacilityPracticeExpenseIndicator)
				return new CmsFssResponse { Amount = 0.0000m, Hcpcs = hcpcs, Modifier = modifier };

			decimal fssAmount =
				((rvu.Work * gpci.Work)
				 + (rvu.TransitionedFacilityPracticeExpense * gpci.PracticeExpense)
				 + (rvu.Malpractice * gpci.Malpractice))
				* 24.6712m;

			return new CmsFssResponse { Amount = Math.Round(fssAmount, 4), Hcpcs = hcpcs, Modifier = modifier };
		}
	}
}
