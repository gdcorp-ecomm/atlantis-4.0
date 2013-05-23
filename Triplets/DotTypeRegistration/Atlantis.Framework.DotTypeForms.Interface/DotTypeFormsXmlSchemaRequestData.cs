﻿using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsXmlSchemaRequestData : RequestData
  {
    public int TldId { get; set; }
    public string Placement { get; set; }

    public DotTypeFormsXmlSchemaRequestData(int tldId, string placement)
    {
      TldId = tldId;
      Placement = placement;
    }

    public override string ToXML()
    {
      var element = new XElement("DotTypeFormsSchemaRequestData");
      element.Add(new XAttribute("tldid", TldId.ToString(CultureInfo.InvariantCulture)));
      element.Add(new XAttribute("placement", Placement));
      return element.ToString();
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
  }
}