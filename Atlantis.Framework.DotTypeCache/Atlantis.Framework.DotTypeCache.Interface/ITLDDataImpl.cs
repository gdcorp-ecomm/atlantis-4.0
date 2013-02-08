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

    List<string> TLDList { get; }

    List<string> GetCustomTLDsByGroupName(string groupName);

    List<string> FilterNonOfferedTLDs(List<string> tldListToFilter);
    HashSet<string> FilterNonOfferedTLDs(HashSet<string> tldSetToFilter);
    bool IsOffered(string tld);
  }
}
