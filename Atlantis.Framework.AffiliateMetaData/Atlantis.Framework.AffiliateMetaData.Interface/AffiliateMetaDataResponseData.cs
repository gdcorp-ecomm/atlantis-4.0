using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AffiliateMetaData.Interface
{
  public class AffiliateMetaDataResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private readonly Dictionary<string, AffiliateData> _affiliateMetaDataDictionary;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public AffiliateMetaDataResponseData(Dictionary<string, AffiliateData> affiliateMetaDataDictionary)
    {
      _affiliateMetaDataDictionary = affiliateMetaDataDictionary;
    }

    public AffiliateMetaDataResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public AffiliateMetaDataResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "AffiliateMetaDataResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    public AffiliateData GetAffiliateByPrefix(string prefix)
    {
      AffiliateData result;
      return _affiliateMetaDataDictionary.TryGetValue(prefix.ToUpperInvariant(), out result) ? result : null;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
