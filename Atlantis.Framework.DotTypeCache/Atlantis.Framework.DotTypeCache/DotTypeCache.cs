using System;
using System.Reflection;
using Atlantis.Framework.DotTypeCache.Interface;
using System.Collections.Generic;
using Atlantis.Framework.Providers.Containers;

namespace Atlantis.Framework.DotTypeCache
{
  public sealed class DotTypeCache
  {
    public static string FileVersion { get; set; }
    public static string InterfaceVersion { get; set; }

    static DotTypeCache()
    {
      try
      {
        object[] fileVersions = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
        if ((fileVersions.Length > 0))
        {
          AssemblyFileVersionAttribute engineFileVersion = fileVersions[0] as AssemblyFileVersionAttribute;
          if (engineFileVersion != null)
          {
            FileVersion = engineFileVersion.Version;
          }
        }

        Type configElementType = typeof(InvalidDotType);
        object[] interfaceFileVersions = configElementType.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
        if ((interfaceFileVersions != null) && (interfaceFileVersions.Length > 0))
        {
          AssemblyFileVersionAttribute interfaceFileVersion = interfaceFileVersions[0] as AssemblyFileVersionAttribute;
          if (interfaceFileVersion != null)
          {
            InterfaceVersion = interfaceFileVersion.Version;
          }
        }
      }
      catch { }        
    }

    private static IDotTypeProvider DotTypes
    {
      get
      {
        return HttpProviderContainer.Instance.Resolve<IDotTypeProvider>();
      }
    }

    public static IDotTypeInfo InvalidDotType
    {
      get
      {
        return DotTypes.InvalidDotType;
      }
    }

    public static IDotTypeInfo GetDotTypeInfo(string dotType)
    {
      return DotTypes.GetDotTypeInfo(dotType);
    }

    public static bool HasDotTypeInfo(string dotType)
    {
      return DotTypes.HasDotTypeInfo(dotType);
    }

    public static int GetExpiredAuctionRegProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetExpiredAuctionRegProductId(registrationLength, domainCount);
    }

    public static int GetExpiredAuctionRegProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetExpiredAuctionRegProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetPreRegProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetPreRegProductId(registrationLength, domainCount);
    }

    public static int GetPreRegProductId(string dotType, string registryId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetPreRegProductId(registryId, registrationLength, domainCount);
    }

    public static int GetRegistrationProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetRegistrationProductId(registrationLength, domainCount);
    }

    public static int GetRegistrationProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetRegistrationProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetTransferProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetTransferProductId(registrationLength, domainCount);
    }

    public static int GetTransferProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetTransferProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetRenewalProductId(string dotType, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetRenewalProductId(registrationLength, domainCount);
    }

    public static int GetRenewalProductId(string dotType, string registrarId, int registrationLength, int domainCount)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetRenewalProductId(registrarId, registrationLength, domainCount);
    }

    public static int GetMinExpiredAuctionRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MinExpiredAuctionRegLength;
    }

    public static int GetMaxExpiredAuctionRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxExpiredAuctionRegLength;
    }

    public static int GetMinPreRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MinPreRegLength;
    }

    public static int GetMaxPreRegLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxPreRegLength;
    }

    public static int GetMinRegistrationLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MinRegistrationLength;
    }

    public static int GetMaxRegistrationLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxRegistrationLength;
    }

    public static int GetMinTransferLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MinTransferLength;
    }

    public static int GetMaxTransferLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxTransferLength;
    }

    public static int GetMinRenewalLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MinRenewalLength;
    }

    public static int GetMaxRenewalLength(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxRenewalLength;
    }

    public static string GetRegistrationFieldsXml(string dotType)
    {
      var dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetRegistrationFieldsXml();
    }
  }
}
