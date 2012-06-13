using System.Linq;
using HealthBus.CmsFss.Data;
using Raven.Client.Indexes;

namespace HealthBus.CmsFss2011.Indices
{
	public class CmsGpciZip2011d_ByZip : AbstractIndexCreationTask<CmsGpciZip2011d>
	{
		public CmsGpciZip2011d_ByZip()
		{
			Map = gpcis => gpcis.Select(gpci => new {gpci.Zip});
		}
	}
}