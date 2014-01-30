using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.Providers.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class TldmlReady : IMonitor
  {
    const int ActiveTldRequest = 635;

    private readonly IDotTypeProvider _dotTypeProvider;
    private readonly ITLDDataCacheProvider _tldDataCacheProvider;
    public TldmlReady()
    {
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ITLDDataCacheProvider, TLDDataCacheProvider>();

      _dotTypeProvider = HttpProviderContainer.Instance.Resolve<IDotTypeProvider>();
      _tldDataCacheProvider = HttpProviderContainer.Instance.Resolve<ITLDDataCacheProvider>();

    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();
      
      var root = new XElement("Monitor");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      try
      {
        var items = qsc.AllKeys.SelectMany(qsc.GetValues, (k, v) => new { key = k, value = v });
        var tldNames = new string[] {};
        foreach (var item in items)
        {
          if (!string.IsNullOrEmpty(item.key) && !string.IsNullOrEmpty(item.value))
          {
            switch (item.key.ToLowerInvariant())
            {
              case "tld":
                var delimiters = new[] { '|', ',' };
                tldNames = item.value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                break;
            }
          }
        }

        ITLDDataImpl tldDataRegistration = _dotTypeProvider.GetTLDDataForRegistration;
        ITLDDataImpl tldDataTransfer = _dotTypeProvider.GetTLDDataForTransfer;
        ITLDDataImpl tldDataBulk = _dotTypeProvider.GetTLDDataForBulk;
        ITLDDataImpl tldDataBulkTransfer = _dotTypeProvider.GetTLDDataForBulkTransfer;

        //var activeTldsRequest = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
        //var activeTldsResponse = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(activeTldsRequest, ActiveTldRequest);

        var activeTlds = _tldDataCacheProvider.GetActiveTlds();

        var offeredTldsRegistration = tldDataRegistration.GetDiagnosticsOfferedTLDFlags(tldNames);
        var offeredTldsTransfer = tldDataTransfer.GetDiagnosticsOfferedTLDFlags(tldNames);
        var offeredTldsBulk = tldDataBulk.GetDiagnosticsOfferedTLDFlags(tldNames);
        var offeredTldsBulkTransfer = tldDataBulkTransfer.GetDiagnosticsOfferedTLDFlags(tldNames);

        foreach (var tldName in tldNames)
        {
          var minRegLength = -1;
          var minXferLength = -1;
          var minRenewalLength = -1;
          var maxRegLength = -1;
          var maxXferLength = -1;
          var maxRenewalLength = -1;

          var regProductId = -1;
          var xferProductId = -1;
          var renewalProductId = -1;

          IDotTypeInfo dotTypeInfo = DotTypeCache.GetDotTypeInfo(tldName);
          if (dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo")
          {
            minRegLength = dotTypeInfo.MinRegistrationLength;
            minXferLength = dotTypeInfo.MinTransferLength;
            minRenewalLength = dotTypeInfo.MinRenewalLength;

            maxRegLength = dotTypeInfo.MaxRegistrationLength;
            maxXferLength = dotTypeInfo.MaxTransferLength;
            maxRenewalLength = dotTypeInfo.MaxRenewalLength;

            try
            {
              regProductId = dotTypeInfo.GetRegistrationProductId(minRegLength, 1);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch { }
            // ReSharper restore EmptyGeneralCatchClause

            try
            {
              xferProductId = dotTypeInfo.GetTransferProductId(minXferLength, 1);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch { }
            // ReSharper restore EmptyGeneralCatchClause

            try
            {
              renewalProductId = dotTypeInfo.GetRenewalProductId(minRenewalLength, 1);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch { }
            // ReSharper restore EmptyGeneralCatchClause
          }

          Dictionary<string, bool> flags;
          
          //var isTldmlReady = dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo" &&
          //                   activeTldsResponse.IsTLDActive(tldName, "availcheckstatus") &&
          //                   (offeredTldsRegistration.TryGetValue(tldName, out flags) ||
          //                    offeredTldsTransfer.TryGetValue(tldName, out flags) ||
          //                    offeredTldsBulk.TryGetValue(tldName, out flags) ||
          //                    offeredTldsBulkTransfer.TryGetValue(tldName, out flags)
          //                   ) &&
          //                   minRegLength > 0 && minXferLength > 0 && minRenewalLength > 0 && maxRegLength > 0 &&
          //                   maxXferLength > 0 && maxRenewalLength > 0 &&
          //                   regProductId > 0 && xferProductId > 0 && renewalProductId > 0;

          bool isTldmlReadyForRegistration = dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo" &&
                             activeTlds.IsTLDActive(tldName, "availcheckstatus") &&
                             offeredTldsRegistration.TryGetValue(tldName, out flags) &&
                             minRegLength > 0 && maxRegLength > 0 &&
                             regProductId > 0;

          bool isTldmlReadyForTransfer = dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo" &&
                             activeTlds.IsTLDActive(tldName, "availcheckstatus") &&
                             offeredTldsTransfer.TryGetValue(tldName, out flags) &&
                             minXferLength > 0 && maxXferLength > 0 &&
                             xferProductId > 0;

          bool isTldmlReadyForBulk = dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo" &&
                             activeTlds.IsTLDActive(tldName, "availcheckstatus") &&
                             offeredTldsBulk.TryGetValue(tldName, out flags) &&
                             minRegLength > 0 && maxRegLength > 0 &&
                             regProductId > 0;

          bool isTldmlReadyForBulkTransfer = dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo" &&
                             activeTlds.IsTLDActive(tldName, "availcheckstatus") &&
                             offeredTldsBulkTransfer.TryGetValue(tldName, out flags) &&
                             minXferLength > 0 && maxXferLength > 0 &&
                             xferProductId > 0;

          var tldmlReady = new XElement("DotType");
          tldmlReady.Add(new XAttribute("value", tldName.ToUpperInvariant()));
          tldmlReady.Add(new XAttribute("TldmlReadyForRegistration", isTldmlReadyForRegistration));
          tldmlReady.Add(new XAttribute("TldmlReadyForTransfer", isTldmlReadyForTransfer));
          tldmlReady.Add(new XAttribute("TldmlReadyForBulk", isTldmlReadyForBulk));
          tldmlReady.Add(new XAttribute("TldmlReadyForBulkTransfer", isTldmlReadyForBulkTransfer));

          var tldInfo = new XElement("TldInfo");
          tldInfo.Add(new XAttribute("TldmlSupported", dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo"));
          tldInfo.Add(new XAttribute("IsActiveTld", activeTlds.IsTLDActive(tldName, "availcheckstatus")));

          tldInfo.Add(new XAttribute("IsOfferedForRegistration", offeredTldsRegistration.TryGetValue(tldName, out flags)));
          tldInfo.Add(new XAttribute("IsOfferedForTransfer", offeredTldsTransfer.TryGetValue(tldName, out flags)));
          tldInfo.Add(new XAttribute("IsOfferedForBulk", offeredTldsBulk.TryGetValue(tldName, out flags)));
          tldInfo.Add(new XAttribute("IsOfferedForBulkTransfer", offeredTldsBulkTransfer.TryGetValue(tldName, out flags)));
          tldmlReady.Add(tldInfo);

          var tldLengths = new XElement("TldmlLengths");
          tldLengths.Add(new XAttribute("MinRegistrationLength", minRegLength.ToString(CultureInfo.InvariantCulture)));
          tldLengths.Add(new XAttribute("MaxRegistrationLength", maxRegLength.ToString(CultureInfo.InvariantCulture)));
          tldLengths.Add(new XAttribute("MinTransferLength", minXferLength.ToString(CultureInfo.InvariantCulture)));
          tldLengths.Add(new XAttribute("MaxTransferLength", maxXferLength.ToString(CultureInfo.InvariantCulture)));
          tldLengths.Add(new XAttribute("MinRenewalLength", minRenewalLength.ToString(CultureInfo.InvariantCulture)));
          tldLengths.Add(new XAttribute("MaxRenewalLength", maxRenewalLength.ToString(CultureInfo.InvariantCulture)));
          tldmlReady.Add(tldLengths);

          var tldProductIds = new XElement("TldmlMinLengthProductIds");
          tldProductIds.Add(new XAttribute("RegistrationProductId", regProductId.ToString(CultureInfo.InvariantCulture)));
          tldProductIds.Add(new XAttribute("TransferProductId", xferProductId.ToString(CultureInfo.InvariantCulture)));
          tldProductIds.Add(new XAttribute("RenewalProductId", renewalProductId.ToString(CultureInfo.InvariantCulture)));
          tldmlReady.Add(tldProductIds);

          root.Add(tldmlReady);          
        }
      }
      catch (Exception ex)
      {
        root.Add(new XElement("error", ex.Message));
      }

      return result;
    }

    private XAttribute GetProcessId()
    {
      return new XAttribute("ProcessId", Process.GetCurrentProcess().Id);
    }

    private XAttribute GetMachineName()
    {
      return new XAttribute("MachineName", Environment.MachineName);
    }

    private XAttribute GetFileVersion()
    {
      return new XAttribute("DotTypeCacheVersion", DotTypeCache.FileVersion);
    }

    private XAttribute GetInterfaceVersion()
    {
      return new XAttribute("DotTypeCacheInterfaceVersion", DotTypeCache.InterfaceVersion);
    }
  }
}
