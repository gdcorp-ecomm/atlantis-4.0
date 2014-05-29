using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.Providers.DomainContactValidation.Interface
{
  public interface IDomainContactError
  {
    string Attribute { get; }

    int Code { get; }

    int MinorCode { get; }

    string Description  { get; }

    int ContactType  { get; }

    string DisplayString { get; }
    
    string DotType  { get; }

    string Country { get; }

    bool ShowError { get; }

    bool RemoveDomain { get; }

    IEnumerable<string> Domains { get; }

    XmlNode Clone();

    string InnerXml { get; }

  }
}
