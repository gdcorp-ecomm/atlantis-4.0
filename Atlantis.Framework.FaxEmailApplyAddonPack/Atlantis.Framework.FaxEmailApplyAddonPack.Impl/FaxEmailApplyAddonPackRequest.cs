using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.FaxEmailApplyAddonPack.Impl.MyaAction;
using Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types;
using Atlantis.Framework.FaxEmailApplyAddonPack.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionAddAttribute.Interface;
using Atlantis.Framework.OrionAddAttribute.Interface.Types;

using Action = Atlantis.Framework.FaxEmailApplyAddonPack.Impl.Types.Action;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl
{
  public class FaxEmailApplyAddonPackRequest : IRequest
  {
    private const string ACTION_NAME = "FaxEmailAddMinutes";
    private const string SHOPPER_NOTE = "FaxThruEmail Minute Pack consumed";
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
      var request = (FaxEmailApplyAddonPackRequestData)requestData;
      FaxEmailApplyAddonPackResponseData response;

      try
      {
        OrionAddAttributeResponseData addAttributeResponse = CreateOrionAttribute(request);
        if (addAttributeResponse == null)
        {
          throw new Exception("Unknown error adding Orion attribute, OrionAddAttributeResponseData is null");
        }

        if (!addAttributeResponse.IsSuccess)
        {
          response = new FaxEmailApplyAddonPackResponseData(addAttributeResponse.ResponseCode, addAttributeResponse.Error, addAttributeResponse.AtlantisException);
        }
        else
        {
          int responseCode = ConsumeAddonPackCredit(request, addAttributeResponse.AttributeUid, config);
          response = new FaxEmailApplyAddonPackResponseData(responseCode, string.Empty);
        }
      }
      catch (AtlantisException atlEx)
      {
        response = new FaxEmailApplyAddonPackResponseData(atlEx);
      }
      catch (Exception ex)
      {
        string data = string.Format("FteAccountUid={0}, FteResourceId={1}, AddonPackResourceId={2}, AddonPackExpireDate={3}, Plid={4}",
                                    request.FteAccountUid,
                                    request.FteResourceId,
                                    request.FaxEmailAddonPack.ResourceId,
                                    request.FaxEmailAddonPack.ExpireDate.ToShortDateString(),
                                    request.PrivateLabelId);
        var atlEx = new AtlantisException(requestData, "FaxEmailApplyAddonPackRequest.RequestHandler", ex.Message, data, ex);
        response = new FaxEmailApplyAddonPackResponseData(atlEx);
      }

      return response;
    }

    private static OrionAddAttributeResponseData CreateOrionAttribute(FaxEmailApplyAddonPackRequestData request)
    {
      var newAttribute = ConstructOrionAttribute(request.FaxEmailAddonPack.PackDetails, request.FaxEmailAddonPack.ExpireDate);
      var addAttrRequest = new OrionAddAttributeRequestData(request.ShopperID,
                                                            request.SourceURL,
                                                            request.OrderID,
                                                            request.Pathway,
                                                            request.PageCount,
                                                            request.PrivateLabelId,
                                                            request.FteAccountUid,
                                                            newAttribute,
                                                            request.Application);
      var response = Engine.Engine.ProcessRequest(addAttrRequest, 453) as OrionAddAttributeResponseData;
      return response;
    }

    private static OrionAttribute ConstructOrionAttribute(Dictionary<string, string> addonPackDetails, DateTime addonPackExpirationDate)
    {
      var addonPackElements = new List<KeyValuePair<string, string>>
      {
        new KeyValuePair<string, string>("expiration_date", addonPackExpirationDate.ToShortDateString())
      };

      if (addonPackDetails.ContainsKey(FaxEmailAddonPack.Minutes))
      {
        addonPackElements.Add(new KeyValuePair<string, string>("num_minutes", addonPackDetails[FaxEmailAddonPack.Minutes]));
      }

      if (addonPackDetails.ContainsKey(FaxEmailAddonPack.Pages))
      {
        addonPackElements.Add(new KeyValuePair<string, string>("num_pages", addonPackDetails[FaxEmailAddonPack.Pages]));
      }

      return new OrionAttribute("minute_pack_addon", addonPackElements);
    }

    private static int ConsumeAddonPackCredit(FaxEmailApplyAddonPackRequestData request, string attributeUid, ConfigElement config)
    {
      //<ACTIONROOT>
      //  <ACTION id="" privatelabelid="" shopper_id="" name=""/>
      //  <FAXEMAIL child_resource_id="" external_resource_id="" child_external_resource_id=""/>
      //  <NOTES>
      //    <SHOPPERNOTE note="" enteredby=""/>
      //    <ACTIONNOTE note="" modifiedby=""/>
      //  </NOTES>
      //</ACTIONROOT>
      //QueueAction("GUID|namespace|id|actiontype|date|xml");
      //QueueActionEvent("GUID|namespace|id|actiontype|date");

      int resourceId = request.FteResourceId;

      string xml = GetActionXml(resourceId, request.ShopperID, request.PrivateLabelId, request.FteAccountUid,
                                request.FaxEmailAddonPack.ResourceId, attributeUid, request.RequestedBy);
      string actionArgs = String.Format("{0}|{1}|{2}|{3}|{4}", Guid.NewGuid(), "FaxEmail", resourceId, ACTION_NAME, DateTime.Now);

      using (var service = new WSCmyaActionService())
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
      }

      return 0;
    }

    public static string GetActionXml(int resourceId, string shopperId, int privateLabelId, string externalResourceId, int addonResourceid, string attributeUid, string enteredby)
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
        FaxEmail = new FaxEmail
        {
          ExternalResourceId = externalResourceId,
          ChildResourceId = addonResourceid,
          ChildExternalResourceId = attributeUid
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
