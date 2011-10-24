using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRenewalOptions.Interface
{
  [Serializable]
  public class GetRenewalOptionsRequestData : RequestData
  {
    private GetRenewalOptionsRequestData()
      : base("", "", "", "", 0)
    {}

    public GetRenewalOptionsRequestData(string shopperID, string sourceURL, string orderID,
                                        string pathway, int pageCount, string resourceID, 
                                        string resourceType, string idType, int privateLabelID)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      ResourceID = resourceID;
      ResourceType = resourceType;
      IDType = idType;
      PrivateLabelID = privateLabelID;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      StringBuilder objectXML = new StringBuilder();
      XmlSerializer serializer = new XmlSerializer(this.GetType());
      serializer.Serialize(new StringWriter(objectXML), this);
      return objectXML.ToString();
    }

    public string ResourceID { get; set; }
    public string ResourceType { get; set; }
    public string IDType { get; set; }
    public int PrivateLabelID { get; set; }
  }
}
