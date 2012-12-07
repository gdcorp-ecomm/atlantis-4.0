using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects
{
  public class TagReplacer
  {
    private readonly Dictionary<string, string> _tagsToReplace;

    public TagReplacer(Dictionary<string,string> tagsToReplace )
    {
      _tagsToReplace = tagsToReplace;
    }

    public string ReplaceTagsIn(string stringToReplace)
    {
      foreach(string replaceFormat in _tagsToReplace.Keys)
      {
        string replacement = _tagsToReplace[replaceFormat];
        stringToReplace = stringToReplace.Replace(replaceFormat, replacement);
      }

      return stringToReplace;
    }

    #region Helper method to determine if doreplace="true" is on the element
    public static bool ReplaceTagOnElement(XElement element)
    {
      bool replaceTagOnElement = false;
      XAttribute replaceAttribute = element.Attribute("doreplace");

      if(replaceAttribute !=null)
      {
        replaceTagOnElement = replaceAttribute.Value.Equals("true", StringComparison.OrdinalIgnoreCase);
      }

      return replaceTagOnElement;
    }
    #endregion
  }
}
