using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLApplicationControl : TLDMLNamespaceElement, ITLDApplicationControl  
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

    private Dictionary<string, ITLDTuiFormGroup> _tuiFormGroups;
    public Dictionary<string, ITLDTuiFormGroup> TuiFormGroups
    {
      get { return _tuiFormGroups; }
    }

    private void Load()
    {
      _dotTypeDescription = string.Empty;
      _landingPageUrl = string.Empty;

      LoadDpp();
      LoadTuiFormGroups();
    }

    private void LoadTuiFormGroups()
    {
      XElement masterFormGroup = NamespaceElement.Descendants("formgroup").FirstOrDefault();
      if (masterFormGroup != null && masterFormGroup.IsEnabled())
      {
        XElement collection = NamespaceElement.Descendants("tuiformgroupcollection").FirstOrDefault();
        if (collection != null)
        {
          var formGroups = collection.Descendants("tuiformgroup");

          _tuiFormGroups = new Dictionary<string, ITLDTuiFormGroup>();

          foreach (var formgroup in formGroups)
          {
            var formTypeValue = string.Empty;
            var formType = formgroup.Attribute("type");
            if (formType != null)
            {
              formTypeValue = formType.Value;
            }

            _tuiFormGroups[formTypeValue] = TldTuiFormGroup.FromFormGroupElement(formgroup);
          }
        }
      }
    }

    private void LoadDpp()
    {
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
