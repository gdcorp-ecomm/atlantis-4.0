﻿using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache
{
  public sealed class DotTypeCache
  {
    // TODO: Obsolete these methods for use of the provider

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

    /// TODO: Obsolete most all of these methods ... callers should Get the dottype using dottypeprovider

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

    public static int GetPreRegistrationProductId(string dotType, int registrationLength, int domainCount, string preRegistrationType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetPreRegistrationProductId(registrationLength, domainCount, preRegistrationType);
    }

    public static int GetPreRegistrationProductId(string dotType, string registryId, int registrationLength, int domainCount, string preRegistrationType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetPreRegistrationProductId(registryId, registrationLength, domainCount, preRegistrationType);
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

    public static int GetMinPreRegistrationLength(string dotType, string preRegistrationType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MinPreRegistrationLength(preRegistrationType);
    }

    public static int GetMaxPreRegistrationLength(string dotType, string preRegistrationType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxPreRegistrationLength(preRegistrationType);
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

    public static int GetMaxRenewalMonthsOut(string dotType)
    {
      IDotTypeInfo dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.MaxRenewalMonthsOut;
    }

    public static string GetAdditionalInfoValue(string dotType, string additionalInfoKey)
    {
      // TODO: how to get additional info for non static?
      return StaticDotTypes.GetAdditionalInfoValue(dotType, additionalInfoKey);
    }

    public static string GetRegistrationFieldsXml(string dotType)
    {
      var dotTypeInfo = GetDotTypeInfo(dotType);
      return dotTypeInfo.GetRegistrationFieldsXml();
    }
  }
}
