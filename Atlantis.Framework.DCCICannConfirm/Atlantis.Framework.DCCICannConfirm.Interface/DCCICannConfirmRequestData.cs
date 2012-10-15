using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.DCCICannConfirm.Interface
{
  public class DCCICannConfirmRequestData : Atlantis.Framework.Interface.RequestData
    {

    private string _appName { set; get; }
    private string iCannEmailGuid { get; set; }
    public int Quantity { get; set; }
    public int? BoundaryRow { get;set;}
    public bool? PagingDirectionForward { get; set; }
    public bool? SortDirectionAscending { get; set; }
    public bool? RepeatRow { get; set; }
    
    public DCCICannConfirmRequestData(string appName,
                                            string shopperId,
                                            string sourceUrl,
                                            string icannEmailGuidParm,
                                            string orderId,
                                            string pathway,
                                            int pageCount,
                                            string dccDomainUser)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
      _appName = appName;
      iCannEmailGuid = icannEmailGuidParm;
    }

      public override string GetCacheMD5()
      {
        MD5 md5 = new MD5CryptoServiceProvider();
        var requestXml = ToXML();
        var data = Encoding.UTF8.GetBytes(requestXml);
        var hash = md5.ComputeHash(data);
        var result = Encoding.UTF8.GetString(hash);
        return result;
      }

      public override string ToXML()
      {
//        <request requestedby="test">
//<email guid="5B927E7D-10AB-4944-8E34-FF8EC0321197"/>
//</request>
        StringBuilder sbRequest = new StringBuilder();
        XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

        xtwRequest.WriteStartElement("request");
        xtwRequest.WriteAttributeString("requestedby", _appName);
        xtwRequest.WriteStartElement("email");
        xtwRequest.WriteAttributeString("guid", iCannEmailGuid);
        xtwRequest.WriteEndElement();
        xtwRequest.WriteEndElement();

        return sbRequest.ToString();
      }
      public string AppName
      {
        get { return _appName; }
      }
    }
}
