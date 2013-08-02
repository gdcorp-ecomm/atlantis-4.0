using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Interface
{
  public abstract class LanguageNamesResponseData : IResponseData
  {
    private readonly Dictionary<int, string> _namesDictionaryById;

    protected LanguageNamesResponseData()
    {
      _namesDictionaryById = new Dictionary<int, string>();
    }

    protected LanguageNamesResponseData(Dictionary<int, string> namesDictionary)
    {
      _namesDictionaryById = namesDictionary ?? new Dictionary<int, string>();
    }

    public string ToXML()
    {
      var element = new XElement(GetType().Name);

      foreach (var keyValue in _namesDictionaryById)
      {
        element.Add(new XElement("item",
          new XAttribute("id", keyValue.Key.ToString(CultureInfo.InvariantCulture)),
          new XAttribute("name", keyValue.Value)));
      }

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public int Count
    {
      get { return _namesDictionaryById.Count; }
    }

    public bool TryGetNameById(int id, out string name)
    {
      return _namesDictionaryById.TryGetValue(id, out name);
    }
  }
}
