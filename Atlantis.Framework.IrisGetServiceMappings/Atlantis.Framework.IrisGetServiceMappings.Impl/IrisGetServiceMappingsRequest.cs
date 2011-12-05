using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.IrisGetServiceMappings.Interface;

namespace Atlantis.Framework.IrisGetServiceMappings.Impl
{
  public class IrisGetServiceMappingsRequest : IRequest
  {
    #region Constants
    private const int _SubscriberId = 112; // corpwebs
    private const int PrivateLabelId_GoDaddy = 1;
    #endregion

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IrisGetServiceMappingsResponseData responseData;
      IrisGetServiceMappingsRequestData requestData = null;
      try
      {
        requestData = (IrisGetServiceMappingsRequestData)oRequestData;

        string response;
        if (requestData.ResellerId == PrivateLabelId_GoDaddy)
        {
          response = DataCache.DataCache.GetAppSetting("IRISWS_PRODUCT_XML");
        }
        else
        {
          using (var ws = new IrisWS.IrisWebService())
          {
            ws.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);
            ws.Url = ((WsConfigElement)oConfig).WSURL;
            response = ws.GetOfferingsListBySubscriberID(_SubscriberId);
          }
        }

        responseData = new IrisGetServiceMappingsResponseData(response, requestData.ParseResponse ? _ParseResponse(response, requestData.ResellerId) : null);
      }
      catch (Exception ex)
      {
        if (oRequestData is IrisGetServiceMappingsRequestData)
        {
          responseData = new IrisGetServiceMappingsResponseData(requestData, ex);
        }
        else
        {
          responseData = new IrisGetServiceMappingsResponseData(oRequestData, ex);
        }
      }

      return responseData;
    }

    #endregion

    private static IrisGetServiceMappingsResponseData.IrisServiceMappings _ParseResponse(string response, int resellerId)
    {
      var results = new IrisGetServiceMappingsResponseData.IrisServiceMappings();

      var xmldoc = new XmlDocument();
      xmldoc.LoadXml(response);

      Dictionary<string, string> nameCorrections = new Dictionary<string, string>(2){
          { "G", "Global" },
          { "O", "Offerings" }
      };

      string serviceIdTag;
      string productGroupTag;
      string friendlyNameTag;
      string categoryTag;
      string rootTag;
      if (resellerId == PrivateLabelId_GoDaddy)
      {
        serviceIdTag = "ID";
        productGroupTag = "PG";
        friendlyNameTag = "Name";
        categoryTag = "C";
        rootTag = "Iris";
      }
      else
      {
        serviceIdTag = "ServiceID";
        productGroupTag = "ProductGroup";
        friendlyNameTag = "FriendlyName";
        categoryTag = "Category";
        rootTag = "IrisServiceMappings";
      }

      XmlNodeList groupings = xmldoc.SelectNodes(rootTag + "/*");

      results.Groupings = new Dictionary<string, IList<IrisGetServiceMappingsResponseData.IrisServiceMapping>>(groupings.Count);
      for (int i = 0; i < groupings.Count; i++)
      {
        var group = groupings[i];
        var groupName = group.Name;
        string correctedName;
        if (resellerId == PrivateLabelId_GoDaddy)
        {
          if (!nameCorrections.TryGetValue(groupName, out correctedName))
          {
            correctedName = groupName;
          }
        }
        else
        {
          correctedName = groupName;
        }

        var mappings = group.SelectNodes(categoryTag);
        results.Groupings.Add(correctedName, _ParseCategoryList(mappings, serviceIdTag, friendlyNameTag, productGroupTag));
      }

      return results;
    }

    private static List<IrisGetServiceMappingsResponseData.IrisServiceMapping> _ParseCategoryList(XmlNodeList list, string serviceIdTag, string friendlyNameTag, string productGroupTag)
    {
      List<IrisGetServiceMappingsResponseData.IrisServiceMapping> outputList = null;
      if (list.Count > 0)
      {
        outputList = new List<IrisGetServiceMappingsResponseData.IrisServiceMapping>(list.Count);
        int i;
        for (i = 0; i < list.Count; i++)
        {
          var item = list[i];

          int serviceId;
          string strServiceId = item.Attributes[serviceIdTag].InnerText;
          if (!int.TryParse(strServiceId, out serviceId))
          {
            throw new Exception(String.Concat("Web service returned invalid ServiceId: \"", strServiceId, "\""));
          }

          string name = item.Attributes[friendlyNameTag].InnerText;
          if (name.Equals(String.Empty))
          {
            throw new Exception("Web service returned empty FriendlyName");
          }

          var productGroups = item.SelectNodes(productGroupTag);
          List<int> productGroupList = null;
          if (productGroups.Count > 0)
          {
            productGroupList = new List<int>(productGroups.Count);
            int j;
            for (j = 0; j < productGroups.Count; j++)
            {
              int productGroupId;
              string strProductGroupId = productGroups[j].InnerText;
              if (!int.TryParse(strProductGroupId, out productGroupId))
              {
                throw new Exception(String.Concat("Web service returned invalid productGroupId: \"", strProductGroupId, "\""));
              }
              productGroupList.Add(productGroupId);
            }
          }

          outputList.Add(new IrisGetServiceMappingsResponseData.IrisServiceMapping()
          {
            FriendlyName = name,
            ServiceId = serviceId,
            ProductGroup = productGroupList
          });
        }
      }
      return outputList;
    }


  }
}
