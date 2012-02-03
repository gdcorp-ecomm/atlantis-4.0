using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.CDS.Tokenizer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.CDS;

namespace Atlantis.Framework.Providers.CDS
{
  public class CDSProvider : ProviderBase, ICDSProvider
  {
    private readonly ISiteContext _siteContext;
    private readonly IShopperContext _shopperContext;

    public CDSProvider(IProviderContainer container) : base(container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _shopperContext = container.Resolve<IShopperContext>();
    }

    #region Implementation of ICDSProvider

    public T GetModel<T>(string query) where T : new()
    {
      return GetModel<T>(query, null);
    }

    public T GetModel<T>(string query, Dictionary<string, string> customTokens) where T : new()
    {

      var data = string.Empty;
      CDSResponseData responseData;
      CDSTokenizer tokenizer = new CDSTokenizer();

      T model = new T();
      var serializer = new JavaScriptSerializer();

      CDSRequestData requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, query);

      try
      {
        responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, 424);
        if (responseData.IsSuccess)
        {
          data = (customTokens != null) ? tokenizer.Parse(responseData.ResponseData, customTokens) : tokenizer.Parse(responseData.ResponseData);
        }
        model = serializer.Deserialize<T>(data);
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
      }
      return model;
    }

    public string GetJSON(string query)
    {
      return GetJSON(query, null);
    }

    public string GetJSON(string query, Dictionary<string, string> customTokens)
    {
      var data = string.Empty;
      CDSResponseData responseData;
      CDSTokenizer tokenizer = new CDSTokenizer();

      CDSRequestData requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, query);

      try
      {
        responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, 424);
        if (responseData.IsSuccess)
        {
          data = (customTokens != null) ? tokenizer.Parse(responseData.ResponseData, customTokens) : tokenizer.Parse(responseData.ResponseData);
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
      }
      return data;
    }

    public string GetUnparsedJSON(string query)
    {
      var data = string.Empty;
      CDSResponseData responseData;
      CDSTokenizer tokenizer = new CDSTokenizer();

      CDSRequestData requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, query);

      try
      {
        responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, 424);
        if (responseData.IsSuccess)
        {
          data = responseData.ResponseData;
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
      }
      return data;
    }

    #endregion
  }
}
