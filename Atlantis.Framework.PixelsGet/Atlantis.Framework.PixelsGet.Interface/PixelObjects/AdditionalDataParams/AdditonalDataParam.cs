using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.AdditionalDataParams
{
  public abstract class AdditionalDataParam
  {
    public virtual string ElementName
    {
      get { throw new NotImplementedException("Must override param"); }
    }
    public virtual string AttributeName
    {
      get { throw new NotImplementedException("Must override param"); }
    }

    public List<string> GetListData(IEnumerable<XElement> elements)
    {
      List<string> listData = new List<string>();

      foreach (XElement element in elements.Descendants(ElementName))
      {
        listData.Add(element.Attribute(AttributeName).Value);
      }

      return listData;
    }
  }
}
