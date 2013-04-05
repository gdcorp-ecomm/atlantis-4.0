using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerCategoriesRequestData : RequestData
  {
    public int ManagerUserId { get; private set; }

    public ManagerCategoriesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int managerUserId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ManagerUserId = managerUserId;
    }

    public override string GetCacheMD5()
    {
      return ManagerUserId.ToString();
    }

    public override string ToXML()
    {
      var element = new XElement("ManagerCategoriesRequestData");
      element.Add(new XAttribute("manageruserid", ManagerUserId.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
