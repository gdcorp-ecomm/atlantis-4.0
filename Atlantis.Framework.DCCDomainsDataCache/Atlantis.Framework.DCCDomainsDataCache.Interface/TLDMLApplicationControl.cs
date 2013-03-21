using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
// ReSharper disable InconsistentNaming
  public class TLDMLApplicationControl : TLDMLNamespaceElement, ITLDApplicationControl  
// ReSharper restore InconsistentNaming
  {

    protected override string Namespace
    {
      get { return "urn:godaddy:ns:applicationcontrol"; }
    }

    protected override string LocalName
    {
      get { return "applicationcontrol"; }
    }

    public TLDMLApplicationControl(XDocument tldmlDoc) : base(tldmlDoc)
    {
      Load();
    }

    private string _dotTypeDescription;
    public string DotTypeDescription
    {
      get { return _dotTypeDescription; }
    }

    private string _landingPageUrl;
    public string LandingPageUrl
    {
      get { return _landingPageUrl; }
    }

    private bool _isMultiRegistry;
    public bool IsMultiRegistry
    {
      get { return _isMultiRegistry; }
    }

    private void Load()
    {
      _dotTypeDescription = string.Empty;
      _landingPageUrl = string.Empty;

      XElement collection = NamespaceElement.Descendants("dpp").FirstOrDefault();
      if (collection != null)
      {
        var tldDescElement = collection.Descendants("tlddescription").FirstOrDefault();
        if (tldDescElement != null)
        {
          var attr = tldDescElement.Attribute("value");
          if (attr != null)
          {
            _dotTypeDescription = attr.Value;
          }
        }

        var landingPageUrlElement = collection.Descendants("landingpageurl").FirstOrDefault();
        if (landingPageUrlElement != null)
        {
          var attr = landingPageUrlElement.Attribute("href");
          if (attr != null)
          {
            _landingPageUrl = attr.Value;
          }
        }

        var multipleRegistrySponsorsElement = collection.Descendants("multipleregistrysponsors").FirstOrDefault();
        if (multipleRegistrySponsorsElement != null)
        {
          var attr = multipleRegistrySponsorsElement.Attribute("enabled");
          if (attr != null)
          {
            _isMultiRegistry = attr.Value.Equals("true");
          }
        }
      }
    }
  }
}
