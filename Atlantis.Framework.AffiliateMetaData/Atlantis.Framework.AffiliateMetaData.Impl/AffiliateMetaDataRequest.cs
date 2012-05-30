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
        List<AffiliateData> affiliateMetaDataList = BuildAffiliateMetaDataList();

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

    private List<AffiliateData> BuildAffiliateMetaDataList()
    {
      string affiliateString = DataCache.DataCache.GetAppSetting("GLOBAL.AFFILIATE.PREFIX.ASSIGNMENT");
      string[] affiliateArray = affiliateString.Split('|');

      List<AffiliateData> affiliateList = new List<AffiliateData>(affiliateArray.Length);

      foreach (string affiliate in affiliateArray)
      {
        string[] affiliateData = affiliate.Split(':');
        affiliateList.Add(new AffiliateData(affiliateData[0].ToUpperInvariant(), affiliateData[1]));
      }

      return affiliateList;
    }
  }
}
