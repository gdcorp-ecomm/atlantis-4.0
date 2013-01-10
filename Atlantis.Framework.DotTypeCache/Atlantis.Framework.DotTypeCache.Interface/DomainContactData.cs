using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  internal class DomainContactData
  {
    public static string DomainContactXml { get; private set; }

    static DomainContactData()
    {
      const string resourcePath = "Atlantis.Framework.DotTypeCache.Interface.DomainContactControls.xml";
      var assembly = Assembly.GetExecutingAssembly();
      using (var streamReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        DomainContactXml = streamReader.ReadToEnd();
      }
    }
  }
}
