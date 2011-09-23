using System.Xml;

namespace Atlantis.Framework.DCCCreateDNS.Interface
{
  public class DnsRecordType
  {
    string _type;         //DNS Record Type. Element is required, but may be null for some methods
    string _name;         //DNS Record Hostname. Element is required, but may be null for some methods
    string _attributeUid; //GUID for this record, null if requesting creation.
    string _data;         //Data portion of the record. Type of data is specific to the record type. Generally an IP or hostname
    string _service;      //SRV service name
    string _protocol;     //SRV protocol name
    int _port;            //SRV Port number
    int _weight;          //SRV Weight value
    int _priority;        //Record priority (MX and SRV)
    int _ttl;             //Time To Live for this record
    string _status;       //Orion status
    //Other attributes on rrecordType seem to be deprecated
    //Docs located at http://10.6.7.100/wiki/doku.php?id=dns:dnsapistructures#rrecordtype

    public DnsRecordType()
    {
      _port = 0;            
      _weight = 0;          
      _priority = 0; 
    }

    public string Type
    {
      get { return _type; }
      set { _type = value; }
    }

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string AttributeUid
    {
      get { return _attributeUid; }
      set { _attributeUid = value; }
    }

    public string Data
    {
      get { return _data; }
      set { _data = value; }
    }

    public string Service
    {
      get { return _service; }
      set { _service = value; }
    }

    public string Protocol
    {
      get { return _protocol; }
      set { _protocol = value; }
    }

    public int Port
    {
      get { return _port; }
      set { _port = value; }
    }

    public int Weight
    {
      get { return _weight; }
      set { _weight = value; }
    }

    public int Priority
    {
      get { return _priority; }
      set { _priority = value; }
    }

    public int TTL
    {
      get { return _ttl; }
      set { _ttl = value; }
    }

    public string Status
    {
      get { return _status; }
      set { _status = value; }
    }

    private XmlNode AddNode(XmlNode parentNode, string sChildNodeName)
    {
      XmlNode childNode = parentNode.OwnerDocument.CreateElement(sChildNodeName);
      parentNode.AppendChild(childNode);
      return childNode;
    }

    private void AddAttribute(XmlNode node, string sAttributeName, string sAttributeValue)
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(sAttributeName);
      node.Attributes.Append(attribute);
      attribute.Value = sAttributeValue;
    }

    public string ToXML()
    {
      XmlDocument recordDoc = new XmlDocument();
      recordDoc.LoadXml("<rrecordtype/>");
      XmlElement oRoot = recordDoc.DocumentElement;
      AddAttribute(oRoot, "type", _type);
      AddAttribute(oRoot, "status", _status);
      AddAttribute(oRoot, "name", _name);
      AddAttribute(oRoot, "attributeUid", _attributeUid);
      AddAttribute(oRoot, "data", _data);
      AddAttribute(oRoot, "service", _service);
      AddAttribute(oRoot, "protocol", _protocol);
      AddAttribute(oRoot, "port", _port.ToString());
      AddAttribute(oRoot, "weight", _weight.ToString());
      AddAttribute(oRoot, "priority", _priority.ToString());
      AddAttribute(oRoot, "ttl", _ttl.ToString());
      AddAttribute(oRoot, "status", _status);
      return recordDoc.InnerXml;
    }

  }
}
