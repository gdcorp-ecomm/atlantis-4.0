using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Atlantis.Framework.BonsaiPlanFeatures.Impl.Types;
using Atlantis.Framework.BonsaiPlanFeatures.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiPlanFeatures.Impl
{
  public class BonsaiPlanFeaturesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      BonsaiPlanFeaturesResponseData response;
      var planFeaturesRequestData = (BonsaiPlanFeaturesRequestData)requestData;

      try
      {
        string planFeatureXml;
        using (var bonsai = new Bonsai.CommerceHelper())
        {
          bonsai.Url = ((WsConfigElement) config).WSURL;
          bonsai.Timeout = (int) planFeaturesRequestData.RequestTimeout.TotalMilliseconds;

          string requestXml = BuildPlanFeatureRequestXml(planFeaturesRequestData);
          planFeatureXml = bonsai.GetPlanFeatureOverridesXml(planFeaturesRequestData.ProductNamespace, requestXml);
        }

        Dictionary<string, string> planFeatures = BuildPlanFeatureDictionary(planFeatureXml);

        response = new BonsaiPlanFeaturesResponseData(planFeatures);
      }
      catch (AtlantisException aEx)
      {
        response = new BonsaiPlanFeaturesResponseData(aEx);
      }
      catch (Exception ex)
      {
        string data = string.Format("upId={0}|namespace={1}|free={2}", planFeaturesRequestData.UnifiedProductId, planFeaturesRequestData.ProductNamespace, planFeaturesRequestData.IsFree);
        var aEx = new AtlantisException(requestData, "BonsaiPlanFeaturesRequest.RequestHandler", ex.Message, data, ex);
        response = new BonsaiPlanFeaturesResponseData(aEx);
      }

      return response;
    }

    private static string BuildPlanFeatureRequestXml(BonsaiPlanFeaturesRequestData requestData)
    {
      var planFeatureRequest = new PlanFeatureOverridesRequest
                                 {
                                   UnifiedProductId = requestData.UnifiedProductId,
                                   IsFree = requestData.IsFree ? 1 : 0,
                                   UnifiedProductIdOverrides = requestData.Overrides
                                 };
      
      var planFeatureRequestXml = new StringBuilder();
      var writerSettings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = false};
      using (var xmlWriter = XmlWriter.Create(planFeatureRequestXml, writerSettings))
      {
        var serializer = new XmlSerializer(typeof(PlanFeatureOverridesRequest));
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);
        serializer.Serialize(xmlWriter, planFeatureRequest, ns);
      }

      return planFeatureRequestXml.ToString();
    }

    private static Dictionary<string, string> BuildPlanFeatureDictionary(string planFeatureXml)
    {
      const string PLANFEATURE_XML_FORMAT = "<AccountElements>{0}</AccountElements>";
      var result = new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(planFeatureXml))
      {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(string.Format(PLANFEATURE_XML_FORMAT, planFeatureXml));

        XmlNodeList acctElements = xmlDoc.SelectNodes("/AccountElements/AccountElement");
        foreach (XmlNode element in acctElements)
        {
          string sName = element.SelectSingleNode("Name").InnerXml;
          string sValue = element.SelectSingleNode("Value").InnerXml;
          result.Add(sName, sValue);
        }
      }

      return result;
    }
  }
}
