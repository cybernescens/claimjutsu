using System.ServiceModel;
using System.ServiceModel.Web;
using HealthBus.CmsFss.Messages;

namespace HealthBus.CmsFss
{
    [ServiceContract]
	public interface ICmsFssNonFacility2011d
    {
        [WebGet(UriTemplate = "{zip}/{hcpcs}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        CmsFssResponse Get(string zip, string hcpcs);

		[WebGet(UriTemplate = "{zip}/{hcpcs}/{modifier}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        CmsFssResponse GetWithMod(string zip, string hcpcs, string modifier);
    }

	[ServiceContract]
	public interface ICmsFssFacility2011d
	{
		[WebGet(UriTemplate = "{zip}/{hcpcs}", ResponseFormat = WebMessageFormat.Xml)]
		[OperationContract]
		CmsFssResponse Get(string zip, string hcpcs);

		[WebGet(UriTemplate = "{zip}/{hcpcs}/{modifier}", ResponseFormat = WebMessageFormat.Xml)]
		[OperationContract]
		CmsFssResponse GetWithMod(string zip, string hcpcs, string modifier);
	}
}