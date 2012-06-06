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
        AffiliateMetaDataRequestData request = (AffiliateMetaDataRequestData)requestData;
        HashSet<string> affiliateMetaDataList = BuildAffiliateMetaDataList(request.PrivateLabelId);

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

    private HashSet<string> BuildAffiliateMetaDataList(int privateLabelId)
    {
      string affiliateString = DataCache.DataCache.GetAppSetting("GLOBAL.AFFILIATE.PREFIX.ASSIGNMENT");
      string[] affiliateArray = affiliateString.Split('|');

      HashSet<string> affiliateSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

      foreach (string affiliate in affiliateArray)
      {
        string[] affiliateData = affiliate.Split(':');
        if (affiliateData != null && affiliateData.Length.Equals(2))
        {
          if (affiliateData[1].Equals("0") || affiliateData[1].Equals(privateLabelId.ToString()))
          {
            affiliateSet.Add(affiliateData[0].ToUpperInvariant());
          }
        }
      }
      return affiliateSet;
    }
  }
}
