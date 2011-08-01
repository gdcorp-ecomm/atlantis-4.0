
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommSwitchPaymentProfile.Interface
{
  public class EcommSwitchPaymentProfileResponseData : IResponseData
  {
    private const string SUCCESS = "SUCCESS";

    public string Result { get; private set; }
    public AtlantisException AtlantisException { get; private set; }
    public bool Successful
    {
      get { return Result == SUCCESS; }
    }

    public EcommSwitchPaymentProfileResponseData(string result)
    {
      Result = result;
    }

    public EcommSwitchPaymentProfileResponseData(AtlantisException atlEx, string result)
    {
      AtlantisException = atlEx;
      Result = result;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }

    #endregion
  }
}
