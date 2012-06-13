using HealthBus.CmsFss.Data;
using Raven.Client.Indexes;
using System.Linq;

namespace HealthBus.CmsFss.Indices
{
	public class CmsRvu2012a_ByHcpcsAndModifier : AbstractIndexCreationTask<CmsRvu2012a>
	{
		public CmsRvu2012a_ByHcpcsAndModifier()
		{
			Map = rvus => rvus.Select(rvu => new {rvu.Hcpcs, rvu.Modifier});
		}
	}
}