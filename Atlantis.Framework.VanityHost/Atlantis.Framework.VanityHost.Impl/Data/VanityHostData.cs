using System.IO;
using System.Reflection;

namespace Atlantis.Framework.VanityHost.Impl.Data
{
  internal class VanityHostData
  {
    public static string VanityHostXml { get; private set; }

    static VanityHostData()
    {
      string resourcePath = "Atlantis.Framework.VanityHost.Impl.Data.vanityhost.xml";
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        VanityHostXml = textReader.ReadToEnd();
      }
    }
  }
}
