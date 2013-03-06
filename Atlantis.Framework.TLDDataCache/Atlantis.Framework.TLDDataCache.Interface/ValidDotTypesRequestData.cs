﻿using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ValidDotTypesRequestData : RequestData
  {
    public string TLD { get; private set; }

    public ValidDotTypesRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      TLD = "0";
    }

    public override string GetCacheMD5()
    {
      return TLD;
    }

    public override string ToXML()
    {
      XElement element = new XElement("ValidDotTypesRequestData");
      element.Add(new XAttribute("tld", TLD));
      return element.ToString();
    }
  }
}
