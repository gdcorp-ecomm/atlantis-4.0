using System.Collections.Generic;
using System.IO;
using System.Xml;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class StaticTld : ITLDTld
  {
    private readonly StaticDotType _staticDotType;
    public StaticTld(StaticDotType staticDotType)
    {
      _staticDotType = staticDotType;
    }

    private List<ITLDLanguageData> _getLanguageList;
    public List<ITLDLanguageData> LanguageDataList
    {
      get
      {
        if (_getLanguageList == null)
        {
          _getLanguageList = new List<ITLDLanguageData>();
          string langXml = DataCache.DataCache.GetCacheData("<GetLanguageListByTLDId><param name=\"tldId\" value=\"" + _staticDotType.TldId.ToString() + "\"/></GetLanguageListByTLDId>");

          using (XmlReader reader = XmlReader.Create(new StringReader(langXml)))
          {
            while (reader.Read())
            {
              if (XmlNodeType.Element == reader.NodeType && "item" == reader.Name.ToLower())
              {
                var langData = new StaticLanguageData(reader.GetAttribute("languageName"), reader.GetAttribute("registryTag"));
                _getLanguageList.Add(langData);
              }
            }
          }
        }
        return _getLanguageList;
      }
    }
  }
}
