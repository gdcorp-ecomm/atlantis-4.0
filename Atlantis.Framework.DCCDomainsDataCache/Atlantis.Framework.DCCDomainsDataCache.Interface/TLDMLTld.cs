using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLTld : TLDMLNamespaceElement, ITLDTld  
  {

    protected override string Namespace
    {
      get { return "urn:godaddy:ns:tld-1.0"; }
    }

    protected override string LocalName
    {
      get { return "tld"; }
    }

    public TLDMLTld(XDocument tldmlDoc) : base(tldmlDoc)
    {
      _languageDataList = LoadLanguageDataList();
    }

    private List<ITLDLanguageData> _languageDataList;
    public List<ITLDLanguageData> LanguageDataList
    {
      get { return _languageDataList; }
    }

    private List<ITLDLanguageData> LoadLanguageDataList()
    {
      List<ITLDLanguageData> result = new List<ITLDLanguageData>();

      XElement languageCollection = NamespaceElement.Descendants("idnlanguagecollection").FirstOrDefault();
      if (languageCollection != null)
      {
        foreach (var language in languageCollection.Descendants("idnlanguage"))
        {
          XAttribute nameAtt = language.Attribute("name");
          XAttribute regTagAtt = language.Attribute("registrytag");

          if (nameAtt != null && regTagAtt != null)
          {
            var languageData = new TldLanguageData(nameAtt.Value, regTagAtt.Value);
            result.Add(languageData);
          }
        }
      }

      return result;
    }

  }
}
