using System.Linq;
using HealthBus.CmsFss.Data;
using Raven.Client.Indexes;

namespace HealthBus.CmsFss.Indices
{
	public class CmsGpciZip2012a_ByZip : AbstractIndexCreationTask<CmsGpciZip2012a>
	{
		public CmsGpciZip2012a_ByZip()
		{
			Map = gpcis => gpcis.Select(gpci => new {gpci.Zip});
		}
	}
}