using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MessagingProcess.Interface
{
  public class MessagingProcessRequestData : RequestData
  {
    private const string _PRIVATELABELIDKEY = "PrivateLabelID";
    private const string _SHOPPERIDKEY = "ShopperID";

    private int _privateLabelId;
    private string _templateType;
    private string _templateNamespace;
    private Dictionary<string, string> _dictionaryItems = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    private List<ResourceItem> _resourceItems = new List<ResourceItem>();

    public int PrivateLabelId
    {
      get { return _privateLabelId; }
    }

    public string TemplateType
    {
      get { return _templateType; }
    }

    public string TemplateNamespace
    {
      get { return _templateNamespace; }
    }

    public void AddDictionaryItem(string name, string value)
    {
      _dictionaryItems[name] = value;
    }

    public void AddResource(ResourceItem resourceItem)
    {
      _resourceItems.Add(resourceItem);
    }

    public MessagingProcessRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int privateLabelId, string templateType, string templateNamespace)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _privateLabelId = privateLabelId;
      _templateType = templateType;
      _templateNamespace = templateNamespace;
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("messageXml");
      xtwRequest.WriteAttributeString("type", _templateType);
      xtwRequest.WriteAttributeString("namespace", _templateNamespace);

      xtwRequest.WriteStartElement("dictionary");

      xtwRequest.WriteStartElement("item");
      xtwRequest.WriteAttributeString("name", _PRIVATELABELIDKEY);
      xtwRequest.WriteValue(_privateLabelId.ToString());
      xtwRequest.WriteEndElement(); // item

      xtwRequest.WriteStartElement("item");
      xtwRequest.WriteAttributeString("name", _SHOPPERIDKEY);
      xtwRequest.WriteValue(ShopperID);
      xtwRequest.WriteEndElement(); // item

      foreach (string key in _dictionaryItems.Keys)
      {
        string value = _dictionaryItems[key];
        if ((key != null) && (value != null) && 
          (key.ToUpperInvariant() != _PRIVATELABELIDKEY.ToUpperInvariant()) &&
          (key.ToUpperInvariant() != _SHOPPERIDKEY.ToUpperInvariant()))
        {
          xtwRequest.WriteStartElement("item");
          xtwRequest.WriteAttributeString("name", key);
          xtwRequest.WriteValue(value);
          xtwRequest.WriteEndElement(); // item
        }
      }

      xtwRequest.WriteEndElement(); // dictionary

      xtwRequest.WriteStartElement("contactpoints");

      HashSet<string> contactPointNamesUsed = new HashSet<string>();

      foreach (ResourceItem resourceItem in _resourceItems)
      {
        foreach (ContactPointItem contactPoint in resourceItem.ContactPoints)
        {
          string name = contactPoint.Name;
          if (!contactPointNamesUsed.Contains(name))
          {
            xtwRequest.WriteStartElement("contactpoint");
            xtwRequest.WriteAttributeString("contact", contactPoint.Name);

            if (contactPoint.ExcludeContactPointType == false)
            {
              xtwRequest.WriteAttributeString("type", contactPoint.Type);
            }

            xtwRequest.WriteEndElement(); // contactpoint
            contactPointNamesUsed.Add(name);
          }
        }
      }

      xtwRequest.WriteEndElement(); // contactpoints

      xtwRequest.WriteStartElement("resources");

      foreach (ResourceItem resourceItem in _resourceItems)
      {
        xtwRequest.WriteStartElement("resource");
        xtwRequest.WriteAttributeString("type", resourceItem.Type);
        xtwRequest.WriteAttributeString("id", resourceItem.Id);

        foreach (string key in resourceItem.Keys)
        {
          AttributeValue attributeItem = resourceItem[key];
          string value = attributeItem.Value;
          if ((key != null) && (value != null))
          {
            xtwRequest.WriteStartElement("attribute");
            xtwRequest.WriteAttributeString("name", key);
            switch (attributeItem.WriteMethod)
            {
              case (int)AttributeValueWriteMethod.Base64:
                xtwRequest.WriteBase64(Encoding.Default.GetBytes(value), 0, Encoding.Default.GetByteCount(value));
                break;
              case (int)AttributeValueWriteMethod.BinHex:
                xtwRequest.WriteBinHex(Encoding.Default.GetBytes(value), 0, Encoding.Default.GetByteCount(value));
                break;
              case (int)AttributeValueWriteMethod.CDataBlock:
                xtwRequest.WriteCData(value);
                break;
              default:
              case (int)AttributeValueWriteMethod.Standard:
                xtwRequest.WriteValue(value);
                break;
            }
            xtwRequest.WriteEndElement(); // attribute
          }
        }

        foreach (ContactPointItem contactPoint in resourceItem.ContactPoints)
        {
          xtwRequest.WriteStartElement("contact");
          xtwRequest.WriteAttributeString("name", contactPoint.Name);

          foreach (string key in contactPoint.Keys)
          {
            string value = contactPoint[key];
            if ((key != null) && (value != null))
            {
              xtwRequest.WriteStartElement("attribute");
              xtwRequest.WriteAttributeString("name", key);
              xtwRequest.WriteValue(value);
              xtwRequest.WriteEndElement(); // attribute
            }
          }
          
          xtwRequest.WriteEndElement(); // contact
        }

        xtwRequest.WriteEndElement(); // resource
      }

      xtwRequest.WriteEndElement(); // resources
      
      xtwRequest.WriteEndElement(); // messageXml

      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MessagingProcessRequest is not cacheable.");
    }
  }
}
