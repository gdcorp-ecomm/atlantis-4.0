using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.CDS.Tokenizer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.CDS;
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.CDS
{
  public class CDSProvider : ProviderBase, ICDSProvider
  {
    private static readonly RenderPipelineManager _cdsWidgetRenderPipelineManager = new RenderPipelineManager();
    private static readonly Regex _validMongoObjectIdRegex = new Regex(@"^[0-9a-fA-F]{24}$", RegexOptions.Compiled);
    private static readonly Regex _widgetContentRegex = new Regex(@"""Content""\s*:\s*""(?<content>[^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.Compiled);

    private readonly ISiteContext _siteContext;
    private readonly IShopperContext _shopperContext;

    static CDSProvider()
    {
      _cdsWidgetRenderPipelineManager.AddRenderHandler(new CDSWidgetConditionRenderHandler());
      _cdsWidgetRenderPipelineManager.AddRenderHandler(new CDSWidgetTokenRenderHandler());
    }

    public CDSProvider(IProviderContainer container) : base(container)
    {
      _siteContext = container.Resolve<ISiteContext>();
      _shopperContext = container.Resolve<IShopperContext>();
    }

    private bool IsValidMongoObjectId(string text)
    {
      bool result = false;
      if (text != null)
      {
        result = _validMongoObjectIdRegex.IsMatch(text);
      }
      return result;
    }

    private string ToQueryString(NameValueCollection nameValuePairs)
    {
      string queryString = string.Empty;
      
      if (nameValuePairs != null)
      {
        StringBuilder queryStringBuilder = new StringBuilder();

        for (int i = 0; i < nameValuePairs.Count; i++)
        {
          queryStringBuilder.AppendFormat(i == 0 ? "{0}={1}" : "&{0}={1}", nameValuePairs.Keys[i], nameValuePairs.Get(i));
        }

        queryString = queryStringBuilder.ToString();
      }

      return queryString;
    }

    private string ParseLegacyTokens(string cdsContent, Dictionary<string, string> customTokens)
    {
      CDSTokenizer tokenizer = new CDSTokenizer();
      return customTokens != null ? tokenizer.Parse(cdsContent, customTokens) : tokenizer.Parse(cdsContent);
    }

    private string ProcessQueryOverrides(string query, out bool bypassCache)
    {
      string finalQuery = query;
      bypassCache = false;

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
            string appendChar = finalQuery.Contains("?") ? "&" : "?";
            finalQuery += string.Concat(appendChar, ToQueryString(queryParams));
          }
        }
      }

      return finalQuery;
    }

    private string ProcessAndRenderRequest(string query, bool bypassCache, IProviderContainer providerContainer, Dictionary<string, string> customTokens)
    {
      CDSRequestData requestData = new CDSRequestData(_shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount, query);

      string finalContent = string.Empty;

      CDSResponseData responseData = bypassCache ? (CDSResponseData)Engine.Engine.ProcessRequest(requestData, CDSProviderEngineRequests.CDSRequestType) : (CDSResponseData)DataCache.DataCache.GetProcessRequest(requestData, CDSProviderEngineRequests.CDSRequestType);

      if (responseData.IsSuccess)
      {
        string content = ParseLegacyTokens(responseData.ResponseData, customTokens);
        
        StringBuilder finalContentBuilder = new StringBuilder(content);

        MatchCollection widgetContentMatches = _widgetContentRegex.Matches(responseData.ResponseData);
        
        foreach (Match widgetContentMatch in widgetContentMatches)
        {
          string rawContent = widgetContentMatch.Groups["content"].Value;

          IRenderContent renderContent = new CDSWidgetRenderContent(rawContent);

          IProcessedRenderContent processedRenderContent = _cdsWidgetRenderPipelineManager.RenderContent(renderContent, providerContainer);

          finalContentBuilder.Replace(rawContent, processedRenderContent.Content);
        }

        finalContent = finalContentBuilder.ToString();
      }

      return finalContent;
    }

    public T GetModel<T>(string query, IProviderContainer providerContainer) where T : new()
    {
      return GetModel<T>(query, providerContainer, null);
    }

    public T GetModel<T>(string query, IProviderContainer providerContainer, Dictionary<string, string> customTokens) where T : new()
    {
      T model = new T();
      var serializer = new JavaScriptSerializer();

      try
      {
        string finalContent = ProcessAndRenderRequest(query, false, providerContainer, customTokens);
        model = serializer.Deserialize<T>(finalContent);
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, _shopperContext.ShopperId, string.Empty, string.Empty, _siteContext.Pathway, _siteContext.PageCount));
      }

      return model;
    }

    public string GetJson(string query, IProviderContainer providerContainer)
    {
      return GetJson(query, providerContainer, null);
    }

    public string GetJson(string query, IProviderContainer providerContainer, Dictionary<string, string> customTokens)
    {
      string finalContent = string.Empty;

      bool bypassCache;
      query = ProcessQueryOverrides(query, out bypassCache);

      try
      {
        finalContent = ProcessAndRenderRequest(query, bypassCache, providerContainer, customTokens);
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(ex.Source, string.Empty, ErrorEnums.GeneralError.ToString(), ex.Message, query, string.Empty, string.Empty, string.Empty, string.Empty, 0));
      }

      return finalContent;
    }
  }
}