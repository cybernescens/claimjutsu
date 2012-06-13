using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using HealthBus.CmsFss.Data;
using HealthBus.CmsFss.Messages;
using HealthBus.CmsFss2011;
using Raven.Client;
using Raven.Client.Linq;

namespace HealthBus.CmsFss
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class CmsFssNonFacility2011d : ICmsFssNonFacility2011d
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

			List<CmsRvu2011d> rvus = Session.Query<CmsRvu2011d>()
				.Where(cms => cms.Hcpcs == hcpcs && cms.Modifier == modifier)
				.Take(2)
				.ToList();

			List<CmsGpciZip2011d> gpcis = Session.Query<CmsGpciZip2011d>()
				.Where(gpc => gpc.Zip == zip)
				.Take(2)
				.ToList();

			//should probably be a 404
			if (rvus.Count < 1 || gpcis.Count < 1)
				return new CmsFssResponse {Amount = 0.0000m, Hcpcs = hcpcs, Modifier = modifier};

			var rvu = rvus[0];
			var gpci = gpcis[0];

			if (rvu.TransitionedNonFacilityPracticeExpenseIndicator)
				return new CmsFssResponse { Amount = 0.0000m, Hcpcs = hcpcs, Modifier = modifier };

			decimal fssAmount =
				((rvu.Work*gpci.Work)
				 + (rvu.TransitionedNonFacilityPracticeExpense*gpci.PracticeExpense)
				 + (rvu.Malpractice*gpci.Malpractice))
				*33.9764m;

			return new CmsFssResponse { Amount = Math.Round(fssAmount, 4), Hcpcs = hcpcs, Modifier = modifier };
		}
	}
}