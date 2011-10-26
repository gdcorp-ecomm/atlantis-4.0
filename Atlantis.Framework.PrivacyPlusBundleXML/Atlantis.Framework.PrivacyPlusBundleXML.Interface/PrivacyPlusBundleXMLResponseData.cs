using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyPlusBundleXML.Interface
{
  public class PrivacyPlusBundleXMLResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public string BundleXML { get; private set; }
    public int RenewalUnifiedProductId { get; private set; }
    public int BundleId { get; private set; }
    private bool _success = false;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public PrivacyPlusBundleXMLResponseData(string customXml, int plRenewalUnifiedProductId, int plBundleId)
    {
      BundleXML = customXml;
      RenewalUnifiedProductId = plRenewalUnifiedProductId;
      BundleId = plBundleId;
      _success = true;
    }

     public PrivacyPlusBundleXMLResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PrivacyPlusBundleXMLResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "PrivacyPlusBundleXMLResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return BundleXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
