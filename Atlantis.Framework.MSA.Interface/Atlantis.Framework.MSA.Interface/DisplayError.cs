using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSA.Interface
{
  public class DisplayError
  {
    public string ErrorTitle { get; set; }
    [XmlElement(ElementName = "ErrorMSG")]
    public string ErrorMsg { get; set; }
  }
}
