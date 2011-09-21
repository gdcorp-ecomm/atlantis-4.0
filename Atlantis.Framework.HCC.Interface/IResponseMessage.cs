using System.ServiceModel;

namespace Atlantis.Framework.HCC.Interface
{
  [ServiceContract(Name = "hccmsg")]
  public interface IHCCResponseMessage
  {
    [OperationContract(Name = "getmsg")]
    string GetResponseMessage();

    [OperationContract(Name = "getstat")]
    string GetResponseStatus();

    [OperationContract(Name = "getcode")]
    int GetResponseStatusCode();
  }
}
