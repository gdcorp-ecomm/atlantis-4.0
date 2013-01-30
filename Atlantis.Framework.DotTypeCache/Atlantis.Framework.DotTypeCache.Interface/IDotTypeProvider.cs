using System.Collections.Generic;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface IDotTypeProvider
  {
    IDotTypeInfo InvalidDotType { get; }
    IDotTypeInfo GetDotTypeInfo(string dotType);
    bool HasDotTypeInfo(string dotType);

    //Don't consume this method. This is to be used only by the monitor to print the TLD info
    Dictionary<string, Dictionary<string, bool>> GetOfferedTLDFlags(OfferedTLDProductTypes tldProductType, string[] tldNames = null);
  }
}
