using System;
using System.Collections.Generic;
using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.FaxEmailApplyAddonPack.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionAddAttribute.Interface;
using Atlantis.Framework.OrionAddAttribute.Interface.Types;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Impl
{
  public class FaxEmailApplyAddonPackRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (FaxEmailApplyAddonPackRequestData) requestData;
      FaxEmailApplyAddonPackResponseData response;

      try
      {
        OrionAddAttributeResponseData addAttributeResponse = CreateOrionAttribute(request, config);
        if (addAttributeResponse == null)
          throw new Exception("Unknown error adding Orion attribute, OrionAddAttributeResponseData is null");

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

    private static OrionAddAttributeResponseData CreateOrionAttribute(FaxEmailApplyAddonPackRequestData request, ConfigElement config)
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
      var addonPackElements = new List<KeyValuePair<string, string>>();
      addonPackElements.Add(new KeyValuePair<string, string>("expiration_date", addonPackExpirationDate.ToShortDateString()));

      if (addonPackDetails.ContainsKey(FaxEmailAddonPack.Minutes))
        addonPackElements.Add(new KeyValuePair<string, string>("num_minutes", addonPackDetails[FaxEmailAddonPack.Minutes]));

      if (addonPackDetails.ContainsKey(FaxEmailAddonPack.Pages))
        addonPackElements.Add(new KeyValuePair<string, string>("num_pages", addonPackDetails[FaxEmailAddonPack.Pages]));

      return new OrionAttribute("minute_pack_addon", addonPackElements);
    }

    private static int ConsumeAddonPackCredit(FaxEmailApplyAddonPackRequestData request, string attributeUid, ConfigElement config)
    {
      int resourceId = request.FteResourceId;
      string externalResourceId = request.FteAccountUid;
      int addonResourceid = request.FaxEmailAddonPack.ResourceId;
      string enteredby = request.RequestedBy;

      //ResourceXml
      //sbResourceNodes.Append("<RESOURCES>");
      //
      //  sbResourceNodes.Append(string.Format("<RESOURCE id=\"{0}\" />", resourceId));
      //
      //sbResourceNodes.Append("</RESOURCES>");

      //ActionXml
      //XmlDocument oXmlDoc = new XmlDocument();
      //oXmlDoc.LoadXml("<FAXEMAIL external_resource_id=\"\"  child_resource_id=\"\" child_external_resource_id=\"\" />");
      //// Set Data
      //XmlNode oRootNode = oXmlDoc.DocumentElement;
      //oRootNode.Attributes.GetNamedItem("external_resource_id").Value = sExternalResourceID;
      //oRootNode.Attributes.GetNamedItem("child_resource_id").Value = sChildResourceID;
      //oRootNode.Attributes.GetNamedItem("child_external_resource_id").Value = sChildExternalResourceID;
      //return oXmlDoc.InnerXml;

      //NoteXml
      //sCreateNoteXml("FaxThruEmail Minute Pack consumed", "AddMinutes", enteredBy)
      //sCreateNoteXml....
      //StringWriter stringWriter = new StringWriter();
      //XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
      //xmlWriter.WriteStartElement("NOTES");
      //xmlWriter.WriteStartElement("SHOPPERNOTE");
      //xmlWriter.WriteAttributeString("note", sShopperNote);
      //xmlWriter.WriteAttributeString("enteredby", sEnteredBy);
      //xmlWriter.WriteEndElement();
      //xmlWriter.WriteStartElement("ACTIONNOTE");
      //xmlWriter.WriteAttributeString("note", "REQUESTEDBY: " + sEnteredBy + Char.ToString('\x0016') + " " + sShopperNote);
      //xmlWriter.WriteAttributeString("modifiedby", 14.ToString());
      //xmlWriter.WriteEndElement();
      //xmlWriter.WriteEndElement();
      //xmlWriter.Close();
      //return stringWriter.ToString();

      //sendQueuedMessage(sResourceXml, sActionXml, sNoteXml, QueueUtil.ActionTypes.FaxEmailAddMinutes);


      return 0;
    }
  }
}
