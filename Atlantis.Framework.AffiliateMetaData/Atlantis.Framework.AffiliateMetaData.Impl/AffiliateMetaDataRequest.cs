using System;
using System.Collections.Generic;
using Atlantis.Framework.AffiliateMetaData.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AffiliateMetaData.Impl
{
  public class AffiliateMetaDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AffiliateMetaDataResponseData responseData = null;

      try
      {
        Dictionary<string, AffiliateData> affiliateMetaDataList = BuildAffiliateMetaDataList();

        responseData = new AffiliateMetaDataResponseData(affiliateMetaDataList);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new AffiliateMetaDataResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new AffiliateMetaDataResponseData(requestData, ex);
      }

      return responseData;
    }

    private Dictionary<string, AffiliateData> BuildAffiliateMetaDataList()
    {
      string affiliateString = DataCache.DataCache.GetAppSetting("GLOBAL.AFFILIATE.PREFIX.ASSIGNMENT");
      string[] affiliateArray = affiliateString.Split('|');

      Dictionary<string, AffiliateData> affiliateList = new Dictionary<string, AffiliateData>(affiliateArray.Length);

      foreach (string affiliate in affiliateArray)
      {
        string[] affiliateData = affiliate.Split(':');
        affiliateList.Add(affiliateData[0].ToUpperInvariant(), new AffiliateData(affiliateData[0].ToUpperInvariant(), affiliateData[1]));
      }

      return affiliateList;
    }
  }
}
