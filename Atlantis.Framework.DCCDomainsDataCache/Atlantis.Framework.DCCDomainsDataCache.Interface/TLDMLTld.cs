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
    }
  }
}
