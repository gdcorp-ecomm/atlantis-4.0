using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Atlantis.Framework.EEMDowngrade.Impl.Types;
using Atlantis.Framework.EEMDowngrade.Interface;
using Atlantis.Framework.Interface;
using Action = Atlantis.Framework.EEMDowngrade.Impl.Types.Action;

namespace Atlantis.Framework.EEMDowngrade.Impl
{
  public class EEMDowngradeRequest : IRequest
  {
    private const string ACTION_NAME = "CampaignBlazerChangePlan";
    private const string SHOPPER_NOTE = "Express Email Marketing Plan Changed: Downgrade";
    private const string MODIFIED_BY = "14";

    private static readonly XmlSerializer m_actionRootSerializer = new XmlSerializer(typeof(ActionRoot));
    private static readonly XmlSerializerNamespaces m_namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName(String.Empty) });
    private static readonly XmlWriterSettings m_xmlWriterSettings = new XmlWriterSettings
    {
      Indent = false,
      NewLineHandling = NewLineHandling.None,
      OmitXmlDeclaration = true,
      CloseOutput = true
    };

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (EEMDowngradeRequestData)requestData;
      EEMDowngradeResponseData response;

      try
      {
        //<ACTIONROOT>
        //  <ACTION id="" privatelabelid="" shopper_id="" name=""/>
        //  <CAMPAIGNBLAZER new_pfid=""/>
        //  <NOTES>
        //    <SHOPPERNOTE note="" enteredby=""/>
        //    <ACTIONNOTE note="" modifiedby=""/>
        //  </NOTES>
        //</ACTIONROOT>
        //QueueAction("GUID|namespace|id|actiontype|date|xml");
        //QueueActionEvent("GUID|namespace|id|actiontype|date");

        string actionArgs = String.Format("{0}|{1}|{2}|{3}|{4}", Guid.NewGuid(), "campblazer", request.BillingResourceId, ACTION_NAME, DateTime.Now);
        string xml = GetActionXml(request.BillingResourceId, request.DowngradeProductId, request.ShopperID, request.PrivateLabelId, request.EnteredBy);

        using (var service = new MyaAction.WSCmyaActionService())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          string result1 = service.QueueAction(String.Concat(actionArgs, "|", xml));
          string result2 = service.QueueActionEvent(actionArgs);
          if (!result1.Contains("SUCCESS"))
          {
            throw new Exception(GetErrorMessage(result1));
          }
          if (!result2.Contains("SUCCESS"))
          {
            throw new Exception(GetErrorMessage(result2));
          }

          response = new EEMDowngradeResponseData();
        }
      }
      catch (AtlantisException atlEx)
      {
        response = new EEMDowngradeResponseData(atlEx);
      }
      catch (Exception ex)
      {
        string data = request == null ? "EEMDowngradeRequestData is null"
          : string.Format("resourceid={0}, downgradePfid={1}, plid={2}, enteredby={3}", request.BillingResourceId, request.DowngradeProductId, request.PrivateLabelId, request.EnteredBy);
        var atlEx = new AtlantisException(requestData, "EEMDowngradeRequest.RequestHandler", ex.Message, data);
        response = new EEMDowngradeResponseData(atlEx);
      }

      return response;
    }

    private string GetActionXml(int resourceId, int downgradeProductId, string shopperId, int privateLabelId, string enteredby)
    {
      string actionNote = "REQUESTEDBY: " + enteredby + "\n " + SHOPPER_NOTE;

      var actionRoot = new ActionRoot
      {
        Action = new Action
        {
          Id = resourceId,
          PrivateLabelId = privateLabelId,
          ShopperId = shopperId,
          Name = ACTION_NAME
        },
        CampaignBlazer = new CampaignBlazer
        {
          DowngradeProductId = downgradeProductId
        },
        Notes = new Notes
        {
          ShopperNote = new ShopperNote
          {
            Note = SHOPPER_NOTE,
            EnteredBy = enteredby
          },
          ActionNote = new ActionNote
          {
            Note = actionNote,
            ModifiedBy = MODIFIED_BY
          }
        }
      };

      var stringWriter = new StringWriter();
      using (var xmlWriter = XmlWriter.Create(stringWriter, m_xmlWriterSettings))
      {
        m_actionRootSerializer.Serialize(xmlWriter, actionRoot, m_namespaces);
      }
      return stringWriter.ToString();
    }

    public static string GetErrorMessage(string statusXml)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(statusXml);
      XmlNode node = doc.SelectSingleNode("/Status/Description");
      return node != null ? node.InnerText : String.Empty;
    }
  }
}
