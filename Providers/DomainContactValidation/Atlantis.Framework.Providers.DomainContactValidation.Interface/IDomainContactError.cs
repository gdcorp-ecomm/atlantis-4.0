using System;
using System.Xml;

namespace Atlantis.Framework.Providers.DomainContactValidation.Interface
{
  public interface IDomainContactError
  {
    string Attribute { get; }

    int Code { get; }

    string Description  { get; }

    int ContactType  { get; }

    string DisplayString { get; }
    
    string DotType  { get; }

    XmlNode Clone();

    string InnerXml { get; }
  }
}
