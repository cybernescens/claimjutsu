using HealthBus.CmsFss.Data;
using Raven.Client.Indexes;
using System.Linq;

namespace HealthBus.CmsFss.Indices
{
	public class CmsRvu2011d_ByHcpcsAndModifier : AbstractIndexCreationTask<CmsRvu2011d>
	{
		public CmsRvu2011d_ByHcpcsAndModifier()
		{
			Map = rvus => rvus.Select(rvu => new {rvu.Hcpcs, rvu.Modifier});
		}
	}
}