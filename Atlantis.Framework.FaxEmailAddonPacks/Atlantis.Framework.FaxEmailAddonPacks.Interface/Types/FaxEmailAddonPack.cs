using System;
using System.Collections.Generic;

namespace Atlantis.Framework.FaxEmailAddonPacks.Interface.Types
{
  public class FaxEmailAddonPack
  {
    public static readonly string Minutes = "minutes";
    public static readonly string Pages = "pages";

    public int ResourceId { get; private set; }
    public DateTime ExpireDate { get; private set; }
    public Dictionary<string, string> PackDetails { get; private set; }

    public FaxEmailAddonPack(int resourceId, DateTime expireDate, Dictionary<string, string> packDetails)
    {
      ResourceId = resourceId;
      ExpireDate = expireDate;
      PackDetails = packDetails;
    }
  }
}
