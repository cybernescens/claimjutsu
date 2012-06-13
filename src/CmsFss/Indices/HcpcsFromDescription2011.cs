using System.Linq;
using HealthBus.CmsFss.Data;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace HealthBus.CmsFss.Indices
{
	public class HcpcsFromDescription2011 : AbstractIndexCreationTask<CmsRvu2011d, HcpcsFromDescription2011.ReduceResult>
	{
		public class ReduceResult
		{
			public string Hcpcs { get; set; }
			public string Description { get; set; }
			public string StatusCode { get; set; }
		}

		public HcpcsFromDescription2011()
		{
			Map = rvus => rvus.Select(rvu => new {rvu.Description, rvu.Hcpcs, rvu.StatusCode});

			Store(x => x.Description, FieldStorage.Yes);
			Store(x => x.Hcpcs, FieldStorage.Yes);
			Store(x => x.StatusCode, FieldStorage.Yes);
		}
	}
}