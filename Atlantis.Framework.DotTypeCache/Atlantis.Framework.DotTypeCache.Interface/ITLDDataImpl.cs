using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDDataImpl
  {
    //This method is only for dignostics purposes
    Dictionary<string, Dictionary<string, bool>> GetDiagnosticsOfferedTLDFlags(string[] tldNames = null);

    HashSet<string> GetTLDsSetForAllFlags(params string[] flagNames);
    HashSet<string> GetOfferedTLDsSetForAllFlags(params string[] flagNames);
    HashSet<string> GetOfferedTLDsSetForAnyFlags(params string[] flagNames);

    List<string> OfferedTLDsList { get; }

    List<string> GetCustomTLDsOfferedByGroupName(string groupName);
    bool IsOffered(string tld);
  }
}
