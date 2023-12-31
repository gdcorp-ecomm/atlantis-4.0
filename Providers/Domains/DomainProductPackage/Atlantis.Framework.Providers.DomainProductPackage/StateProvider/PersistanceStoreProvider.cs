﻿using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.Interface;
using Newtonsoft.Json;

namespace Atlantis.Framework.Providers.DomainProductPackage.StateProvider
{
  public class PersistanceStoreProvider : ProviderBase, IPersistanceStoreProvider
  {
    private const string SESSION_PACKAGE_ID = "DomainProductPackageSerializer.DomainProductPackage";

    public PersistanceStoreProvider(IProviderContainer container) : base(container){}

    public static IPersistanceStore SessionState = new PersistanceStore();

    public bool TryGetDomainProductPackages(out IEnumerable<IDomainRegistrationProductPackageGroup> domainProductPackages)
    {
      bool success;
      domainProductPackages = new List<IDomainRegistrationProductPackageGroup>(8);
      var packageDataSerializedString = string.Empty;

      try
      {
        packageDataSerializedString = SessionState.Get(SESSION_PACKAGE_ID);

        if (!string.IsNullOrEmpty(packageDataSerializedString))
        {
          IEnumerable<PackageData> packageDataItems;
          if (Serializer.TryGetPackageDataItems(packageDataSerializedString, out packageDataItems))
          {
            domainProductPackages = PackageBuilder.PackageDataToDomainRegistrationProduct(packageDataItems.ToList(), Container);
          }
        }

        success = domainProductPackages.Any();
      }
      catch (Exception ex)
      {
        success = false;
        Engine.Engine.LogAtlantisException(new AtlantisException("PersistanceStoreProvider.TryGetDomainProductPackages", 0, ex.ToString(), packageDataSerializedString));
      }

      return success;
    }

    public void SaveDomainProductPackages(IEnumerable<IDomainRegistrationProductPackageGroup> domainRegistratoinProductPackageGroup)
    {
      var packageDataItems = PackageBuilder.DomainRegistrationProductToPackageData(domainRegistratoinProductPackageGroup);

      var output = JsonConvert.SerializeObject(packageDataItems);

      SessionState.Save(SESSION_PACKAGE_ID, output);
    }
  }
}
