using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Interface
{
  public class NonUnifiedPfidResponseData : IResponseData
  {
    public static NonUnifiedPfidResponseData NotFound { get; private set; }

    static NonUnifiedPfidResponseData()
    {
      NotFound = new NonUnifiedPfidResponseData(0);
    }

    public int NonUnifiedPfid { get; private set; }

    public static NonUnifiedPfidResponseData FromNonUnifiedPfid(int nonUnifiedPfid)
    {
      if (nonUnifiedPfid == 0)
      {
        return NotFound;
      }
      else
      {
        return new NonUnifiedPfidResponseData(nonUnifiedPfid);
      }
    }

    private NonUnifiedPfidResponseData(int nonUnifiedPfid)
    {
      NonUnifiedPfid = nonUnifiedPfid;
    }

    public string ToXML()
    {
      XElement element = new XElement("NonUnifiedPfidResponseData");
      element.Add(new XAttribute("pfid", NonUnifiedPfid.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
