using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.EcommDelayedPayment.Interface
{
  public class Payments
  {
    public Profile CurrentProfile { get; set; }
    public CCPayment CurrentCCPayment { get; set; }

    public Payments()
    {
      CurrentProfile = new Profile();
      CurrentCCPayment = new CCPayment();
    }
    public void ToXML(XmlTextWriter xtwRequest)
    {
      xtwRequest.WriteStartElement("Payments");

      CurrentProfile.ToXML(xtwRequest);
      CurrentCCPayment.ToXML(xtwRequest);

      xtwRequest.WriteEndElement();
    }

  }
}
