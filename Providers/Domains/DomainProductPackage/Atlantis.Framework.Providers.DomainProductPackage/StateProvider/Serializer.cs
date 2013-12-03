using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Newtonsoft.Json;

namespace Atlantis.Framework.Providers.DomainProductPackage.StateProvider
{
  internal class Serializer
  {
    public static bool TryGetPackageDataItems(string packageDataSerializedString, out IEnumerable<PackageData> packageDataItems)
    {
      var isSuccess = true;
      packageDataItems = new List<PackageData>(0);

      try
      {
        packageDataItems = JsonConvert.DeserializeObject<IEnumerable<PackageData>>(packageDataSerializedString);
      }
      catch (Exception ex)
      {
        isSuccess = false;
        Engine.Engine.LogAtlantisException(new AtlantisException("DomainProductPackageStateProvider.TryRebuildDomainProductPackages", 0, ex.ToString(), packageDataSerializedString));
      }

      return isSuccess;
    }

    public static string SerializePackageDataItems(IEnumerable<PackageData> packageDataItems)
    {
      var output = JsonConvert.SerializeObject(packageDataItems);

      return output;
    }
  }
}
