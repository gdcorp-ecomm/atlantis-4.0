using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCC.Interface.Constants;
using Atlantis.Framework.HCC.Interface.DomainSettings;
using Atlantis.Framework.HCCSetupOptions.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCSetupOptions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetupOptions.Impl
{
  public class HCCSetupOptionsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      var apiRequestData = requestData as HCCSetupOptionsRequestData;

      try
      {
        var ws = new HCCAPIWebService.HCCAPIService {Url = ((WsConfigElement) config).WSURL};
        if (apiRequestData != null)
        {
          ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
          var apiResponse = ws.GetAccountSetupOptions(apiRequestData.AccountUid);

          if (apiResponse != null)
          {
            responseData = new HCCSetupOptionsResponseData(GetHCCResponse(apiResponse));
          }
          else
          {
            var ex = new AtlantisException(apiRequestData,
                                           "HCCSetupOptionsRequest.RequestHandler",
                                           "API Response is null or AccountList is null",
                                           string.Empty);

            responseData = new HCCSetupOptionsResponseData(apiRequestData, ex);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCSetupOptionsResponseData(apiRequestData, ex);
      }

      return responseData;
    }

    HCCSetupOptionsResponse GetHCCResponse(HCCAPIWebService.AccountSetupOptionsResponse apiResponse)
    {
      var response = new HCCSetupOptionsResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);

      if (apiResponse.OptionGroups != null)
      {
        response.SetHCCOptionGroups(GetHCCOptionGroups(apiResponse.OptionGroups));
      }

      return response;
    }

    static int GetOptionType(OptionType optionType)
    {
      var resultType = HCCOptionType.UNKNOWN;

      switch (optionType)
      {
        case OptionType.Checkbox:
          resultType = HCCOptionType.CHECKBOX;
          break;
        case OptionType.Radio:
          resultType = HCCOptionType.RADIO;
          break;
        default:
          resultType = HCCOptionType.UNKNOWN;
          break;
      }

      return resultType;
    }

    ReadOnlyCollection<HCCOptionGroup> GetHCCOptionGroups(OptionGroup[] optionGroups)
    {
      var hccOptionGroups = new List<HCCOptionGroup>(optionGroups.Length);

      foreach (OptionGroup optionGroup in optionGroups)
      {
        if (optionGroup != null)
        {
          var hccOptionGroup = new HCCOptionGroup();
          if (optionGroup.Options != null)
          {
            hccOptionGroup.SetOptionListItems(GetHCCOptionListItems(optionGroup.Options));
          }
          hccOptionGroup.HelpArticleID = optionGroup.HelpArticleID;
          hccOptionGroup.ListType = GetOptionType(optionGroup.ListType);
          hccOptionGroup.Text = optionGroup.Text ?? string.Empty;
          hccOptionGroup.Title = optionGroup.Title ?? string.Empty;

          hccOptionGroups.Add(hccOptionGroup);
        }
        
      }

      return hccOptionGroups.AsReadOnly();
    }

    static ReadOnlyCollection<HCCOptionListItem> GetHCCOptionListItems(OptionListItem[] optionListItems)
    {
      var listItems = new List<HCCOptionListItem>(optionListItems.Length);

      listItems.AddRange(optionListItems.Select(item => new HCCOptionListItem {Selected = item.Selected, Text = item.Text, Value = item.Value}));

      return listItems.AsReadOnly();
    }
  }
}
