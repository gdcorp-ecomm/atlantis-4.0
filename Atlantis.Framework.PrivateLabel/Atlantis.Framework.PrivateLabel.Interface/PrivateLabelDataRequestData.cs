using Atlantis.Framework.PrivateLabel.Interface.Base;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.PrivateLabel.Interface
{
  public class PrivateLabelDataRequestData : RequestDataUsingPrivateLabelId
  {
    public int DataCategoryId { get; private set; }

    [Obsolete("Please use the simpler constructor.")]
    public PrivateLabelDataRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, int dataCategoryId)
      : this(privateLabelId, dataCategoryId)
    {
    }

    public PrivateLabelDataRequestData(int privateLabelId, int dataCategoryId)
      :base(privateLabelId)
    {
      DataCategoryId = dataCategoryId;
    }

    public override string GetCacheMD5()
    {
      return string.Concat(PrivateLabelId.ToString(), ":", DataCategoryId.ToString());
    }

    public override string ToXML()
    {
      XElement element = new XElement("PrivateLabel");
      element.Add(new XAttribute("privatelabelid", PrivateLabelId.ToString()));
      element.Add(new XAttribute("categoryid", DataCategoryId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
