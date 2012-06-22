using System.Xml.Linq;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCContentItem
  {
    public string Name { get; private set; }
    public string Location { get; private set; }
    public string Content { get; private set; }

    internal PCContentItem(XElement itemElement)
    {
      Name = null;
      Location = null;
      Content = null;

      if (itemElement != null)
      {
        XAttribute nameAtt = itemElement.Attribute("name");
        if (nameAtt != null)
        {
          Name = nameAtt.Value;
        }

        XAttribute locationAtt = itemElement.Attribute("location");
        if (locationAtt != null)
        {
          Location = locationAtt.Value;
        }

        if (!string.IsNullOrEmpty(itemElement.Value))
        {
          Content = itemElement.Value;
        }
      }
    }
  }
}
