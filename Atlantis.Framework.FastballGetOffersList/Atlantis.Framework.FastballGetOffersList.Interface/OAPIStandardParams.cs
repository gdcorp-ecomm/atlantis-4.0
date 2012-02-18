using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballGetOffersList.Interface
{
  public class OapiStandardParams
  {
    public int ApplicationId { get; set; }
    public int PrivateLabelId { get; set; }
    public string ShopperId { get; set; }
    public string Placement { get; set; }
    public string OptionalIscCode { get; set; }
    public DateTime? QaSpoofDate { get; set; }
    public string DisplayCurrency { get; set; }
    public string TransactionalCurrency { get; set; }
    public string Platform { get; set; }
    public string RepVersion { get; set; }
  }
}
