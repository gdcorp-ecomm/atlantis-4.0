using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AffiliateMetaData.Interface
{
  public class AffiliateMetaDataResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    private readonly HashSet<string> _affiliateMetaDataItems;

    #endregion

    public AffiliateMetaDataResponseData(HashSet<string> affiliateMetaDataList)
    {
      _affiliateMetaDataItems = affiliateMetaDataList;
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

    public bool AffiliateItemsContains(string affiliateType)
    {
      return _affiliateMetaDataItems.Contains(affiliateType);
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
