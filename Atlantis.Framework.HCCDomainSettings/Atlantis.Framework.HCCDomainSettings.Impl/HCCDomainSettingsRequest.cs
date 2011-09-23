using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Atlantis.Framework.HCC.Interface.Constants;
using Atlantis.Framework.HCC.Interface.DomainSettings;
using Atlantis.Framework.HCCDomainSettings.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCDomainSettings.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCDomainSettings.Impl
{
  public class HCCDomainSettingsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCDomainSettingsRequestData apiRequestData = requestData as HCCDomainSettingsRequestData;

      try
      {
        HCCAPIService ws = new HCCAPIWebService.HCCAPIService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.DomainSettingsResponse apiResponse = ws.GetDomainSettingsOptions(apiRequestData.AccountUid, apiRequestData.Domain);

        if (apiResponse != null)
        {
          responseData = new HCCDomainSettingsResponseData(GetHCCResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(apiRequestData,
            "HCCDomainSettingsRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCDomainSettingsResponseData(apiRequestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCDomainSettingsResponseData(apiRequestData, ex);
      }

      return responseData;
    }

    HCCDomainSettingsResponse GetHCCResponse(HCCAPIWebService.DomainSettingsResponse apiResponse)
    {
      HCCDomainSettingsResponse response = new HCCDomainSettingsResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
      response.Errors = apiResponse.Errors;
      response.Texts = apiResponse.Text;
      
      if (apiResponse.OptionsList != null)
      {
        response.OptionGroup = new HCCOptionGroup();
        response.OptionGroup.HelpArticleID = apiResponse.OptionsList.HelpArticleID;
        response.OptionGroup.ListType = GetOptionType(apiResponse.OptionsList.ListType);
        response.OptionGroup.SetOptionListItems(GetHCCOptionListItems(apiResponse.OptionsList.Options));
        response.OptionGroup.Text = apiResponse.OptionsList.Text;
        response.OptionGroup.Title = apiResponse.OptionsList.Title;
      }

      return response;
    }

    int GetOptionType(OptionType optionType)
    {
      int resultType = HCCOptionType.UNKNOWN;

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

    ReadOnlyCollection<HCCOptionListItem> GetHCCOptionListItems(OptionListItem[] optionListItems)
    {
      List<HCCOptionListItem> listItems = new List<HCCOptionListItem>(optionListItems.Length);

      foreach (OptionListItem item in optionListItems)
      {
        HCCOptionListItem listItem = new HCCOptionListItem();
        listItem.Selected = item.Selected;
        listItem.Text = item.Text;
        listItem.Value = item.Value;

        listItems.Add(listItem);
      }

      return listItems.AsReadOnly();
    }
  }


}
