using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AffiliateMetaData.Interface
{
  public class AffiliateMetaDataResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;
    private readonly Dictionary<string, AffiliateData> _affiliateMetaDataDictionary;
    private readonly List<AffiliateData> _affiliateMetaDataList;

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public IEnumerable<AffiliateData> AffiliateMetaDataItems
    {
      get { return _affiliateMetaDataList; }
    }
    #endregion

    public AffiliateMetaDataResponseData(List<AffiliateData> affiliateMetaDataList)
    {
      _affiliateMetaDataList = affiliateMetaDataList;
      _affiliateMetaDataDictionary = new Dictionary<string,AffiliateData>(affiliateMetaDataList.Capacity);
      _affiliateMetaDataList.ForEach(md => _affiliateMetaDataDictionary.Add(md.Prefix, md));
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
