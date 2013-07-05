using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  [XmlRoot(ElementName = "Data")]
  public class XmlCDSPlaceHolderData : XmlPlaceHolderData, ICDSPlaceHolderData
  {
    [XmlAttribute(AttributeName = "app")]
    public string App { get; set; }
  }
}
