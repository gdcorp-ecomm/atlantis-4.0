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

    public string GetJSON(string query)
    {
      return GetJSON(query, null);
    }

    public string GetJSON(string query, Dictionary<string, string> customTokens)
    {
      var data = string.Empty;
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
          data = (customTokens != null) ? tokenizer.Parse(responseData.ResponseData, customTokens) : tokenizer.Parse(responseData.ResponseData);
        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
      }
      return data;
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
