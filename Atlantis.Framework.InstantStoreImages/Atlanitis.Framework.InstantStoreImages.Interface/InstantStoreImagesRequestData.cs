using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace Atlantis.Framework.InstantStoreImages.Interface
{
  public class InstantStoreImagesRequestData : RequestData
  {
    
    public InstantStoreImagesRequestData(string sShopperID,
                  string sSourceURL,
                  string sOrderID,
                  string sPathway,
                  int iPageCount)
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(8);
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("ShopperID", ShopperID);
      xtwRequest.WriteAttributeString("SourceURL", SourceURL);
      xtwRequest.WriteAttributeString("OrderID", OrderID);
      xtwRequest.WriteAttributeString("Pathway", Pathway);
      xtwRequest.WriteAttributeString("PageCount", System.Convert.ToString(PageCount));
      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();

      oMD5.Initialize();

      byte[] stringBytes

      = System.Text.ASCIIEncoding.ASCII.GetBytes(ShopperID);

      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);

      string sValue = BitConverter.ToString(md5Bytes, 0);

      return sValue.Replace("-", "");
    }

  }
}
