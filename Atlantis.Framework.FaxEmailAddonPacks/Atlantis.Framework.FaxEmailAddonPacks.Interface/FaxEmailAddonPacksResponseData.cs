using System.Collections.Generic;
using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FaxEmailAddonPacks.Interface
{
  public class FaxEmailAddonPacksResponseData : IResponseData
  {
    public List<FaxEmailAddonPack> AddonPacks { get; private set; }
    public AtlantisException AtlantisException { get; private set; }

    public bool IsSuccess
    {
      get { return AtlantisException == null; }
    }

    public FaxEmailAddonPacksResponseData(List<FaxEmailAddonPack> addonPacks)
    {
      AddonPacks = addonPacks;
    }

    public FaxEmailAddonPacksResponseData(AtlantisException atlantisException)
    {
      AtlantisException = atlantisException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }
  }
}
