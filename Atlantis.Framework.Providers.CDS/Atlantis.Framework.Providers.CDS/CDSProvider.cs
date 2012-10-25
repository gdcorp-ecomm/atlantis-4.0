using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.CDS.Tokenizer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.CDS;
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Linq;
using Atlantis.Framework.Tokens.Interface;

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

      var parsedData1 = string.Empty;
      var parsedData2 = string.Empty;
      CDSResponseData responseData;
      CDSTokenizer tokenizer = new CDSTokenizer();

      T model = new T();
      var serializer = new JavaScriptSerializer();

      CDSRequestData requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, query);

      try
      {
        responseData = (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.CDSRequestType );
        if (responseData.IsSuccess)
        {
          //old token framework - this line will parse tokens that are in old format. Eg. {{product::3604::price::keepdecimal::yearly}}
          //Eventually after all tokens have been converted into the format, this line would be removed from here.
          parsedData1 = (customTokens != null) ? tokenizer.Parse(responseData.ResponseData, customTokens) : tokenizer.Parse(responseData.ResponseData);

          //new token framework - this line will parse token that are in new format.  Eg. [@T[AdCreditShowHide:{property: "google", html: "some html here..."}]@T]
          TokenEvaluationResult result = TokenManager.ReplaceTokens(parsedData1, Container, out parsedData2);
        }
        model = serializer.Deserialize<T>(parsedData2);
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, _shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount));
      }
      return model;
    }



    public string GetJSON(string query)
    {
      return GetJSON(query, null);
    }

    public string GetJSON(string query, Dictionary<string, string> customTokens)
    {
      var parsedData1 = string.Empty;
      var parsedData2 = string.Empty;
      CDSResponseData responseData;

      bool bypassCache = false;
      if (HttpContext.Current != null)
      {
        DateTime activeDate;
        NameValueCollection queryString = HttpContext.Current.Request.QueryString;
        string docId = queryString["docid"];
        string qsDate = queryString["activedate"];
        if ((DateTime.TryParse(qsDate, out activeDate) || IsValidMongoObjectId(docId)) && _siteContext.IsRequestInternal)
        {
          bypassCache = true;
          NameValueCollection queryParams = new NameValueCollection();
          if (activeDate != default(DateTime))
          {
            queryParams.Add("activedate", activeDate.ToString("O"));
          }
          if (IsValidMongoObjectId(docId))
          {
            queryParams.Add("docid", docId);
          }
          if (queryParams.Count > 0)
          {
            string appendChar = query.Contains("?") ? "&" : "?";
            query += string.Concat(appendChar, ToQueryString(queryParams));
          }
        }
      }

      CDSRequestData requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, query);

      try
      {
        responseData = bypassCache ? (CDSResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.CDSRequestType) : (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.CDSRequestType);
        if (responseData.IsSuccess)
        {
          CDSTokenizer tokenizer = new CDSTokenizer();

          //old token framework - this line will parse tokens that are in old format. Eg. {{product::3604::price::keepdecimal::yearly}}
          //Eventually after all tokens have been converted into the format, this line would be removed from here.
          parsedData1 = (customTokens != null) ? tokenizer.Parse(responseData.ResponseData, customTokens) : tokenizer.Parse(responseData.ResponseData);

          //new token framework - this line will parse token that are in new format.  Eg. [@T[AdCreditShowHide:{property: "google", html: "some html here..."}]@T]
          TokenEvaluationResult result = TokenManager.ReplaceTokens(parsedData1, Container, out parsedData2);
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
      }
      return parsedData2;
    }

    private bool IsValidMongoObjectId(string text)
    {
      bool result = false;
      if (text != null)
      {
        string pattern = @"^[0-9a-fA-F]{24}$";
        result = Regex.IsMatch(text, pattern);
      }
      return result;
    }

    private string ToQueryString(NameValueCollection nvc)
    {
      return string.Join("&", nvc.AllKeys.SelectMany(key => nvc.GetValues(key).Select(value => string.Format("{0}={1}", key, value))).ToArray());
    }

    #endregion
  }
}
