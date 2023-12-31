﻿using Atlantis.Framework.Interface;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using Newtonsoft.Json;

namespace Atlantis.Framework.BasePages.Json
{
  public abstract class AtlantisContextJsonDataBasePage : AtlantisContextBasePage
  {
    public AtlantisContextJsonDataBasePage() : base()
    {
      AllowCrossDomainJson = JsonSecurity.AllowCrossDomainJsonDefault;
    }

    protected enum RenderModeType
    {
      Undetermined,
      Json,
      DebugPage,
      DebugJson
    }

    protected enum JsonSerializationType
    {
      JavaScript,
      DataContract
    }

    private RenderModeType _renderMode = RenderModeType.Undetermined;

    protected RenderModeType RenderMode
    {
      get
      {
        if (_renderMode == RenderModeType.Undetermined)
        {
          _renderMode = RenderModeType.Json;
          if (SiteContext.IsRequestInternal)
          {
            if (Request["render"] != null)
            {
              string render = Request["render"];
              if (string.Compare(render, "page", true) == 0)
              {
                _renderMode = RenderModeType.DebugPage;
              }
              else if (string.Compare(render, "json", true) == 0)
              {
                _renderMode = RenderModeType.DebugJson;
              }
            }
          }
        }

        return _renderMode;
      }
      set
      {
        _renderMode = value;
      }
    }

    protected bool AllowCrossDomainJson { get; set; }

    protected virtual string CallBack
    {
      get
      {
        string result = string.Empty;
        if ((AllowCrossDomainJson) && (Request["callback"] != null))
        {
          string callback = Request["callback"];
          if (JsonSecurity.IsCallbackValid(callback))
          {
            result = callback;
          }
        }

        return result;
      }
    }

    protected abstract string GetSerializedJson();

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    private int _maxJsonStringLength = 2097152;
    protected int MaxJsonStringLength
    {
      get
      {
        return _maxJsonStringLength;
      }
      set
      {
        _maxJsonStringLength = value;
      }
    }

    protected string SerializeToJson<T>(T objectToSerialize)
    {
      var sb = new StringBuilder(1000);

      using (var sw = new StringWriter(sb))
      {
        var jsonText = new JsonTextWriter(sw);

        JsonSerializer serializer;
      
        
          serializer = JsonSerializer.Create();


        serializer.Serialize(jsonText, objectToSerialize, typeof(T));
        jsonText.Flush();
      }

      return sb.ToString();

    }
    protected string SerializeToJson<T>(T dataContractObject, JsonSerializationType serializerType)
    {
      string resultJson = string.Empty;
      switch (serializerType)
      {
        case JsonSerializationType.DataContract:
          resultJson = SerializeToJson(dataContractObject);
          break;
        case JsonSerializationType.JavaScript:
          JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
          jsonSerializer.MaxJsonLength = MaxJsonStringLength;
          resultJson = jsonSerializer.Serialize(dataContractObject);
          break;
      }
      return resultJson;
    }


    protected void RenderPageContent(HtmlTextWriter writer)
    {
      base.Render(writer);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (RenderMode == RenderModeType.DebugPage)
      {
        RenderPageContent(writer);
      }
      else if (RenderMode == RenderModeType.DebugJson)
      {
        string json = GetSerializedJson();
        writer.WriteEncodedText(json);
      }
      else
      {
        WriteSerializedJSON(writer, string.Empty);
      }
    }

    protected virtual void WriteSerializedJSON(HtmlTextWriter writer, string errorMessage)
    {
      Response.ContentType = "application/json";
      string json = string.Empty;

      if (string.IsNullOrEmpty(errorMessage))
      {
        json = GetSerializedJson();
      }
      else
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"Error\":\"");
        sb.Append(Server.HtmlEncode(errorMessage.Replace('\n', ' ').Replace('\r', ' ')));
        sb.Append("\"}");
        json = sb.ToString();
      }

      if (CallBack.Length > 0)
      {
        Response.ContentType = "text/javascript";
        json = string.Concat(CallBack, "(", json, ")");
      }

      if (writer != null)
      {
        writer.Write(json);
      }
      else
      {
        Response.Write(json);
      }
    }

    protected override void OnError(EventArgs e)
    {
      string errorMessage = "Error";
      Exception ex = Server.GetLastError();
      Server.ClearError();

      try
      {
        if (ex != null)
        {
          Exception baseEx = ex.GetBaseException();
          if (baseEx != null)
          {
            ex = baseEx;
          }

          if (SiteContext.IsRequestInternal)
          {
            errorMessage = ex.ToString();
          }

          string logMessage = ex.Message + Environment.NewLine + ex.StackTrace;
          AtlantisException aex = new AtlantisException("JsonDataBasePage.OnError", Request.Url.ToString(), "0", logMessage, string.Empty, ShopperContext.ShopperId, string.Empty, Request.UserHostAddress, SiteContext.Pathway, SiteContext.PageCount);
          Engine.Engine.LogAtlantisException(aex);
        }

        WriteSerializedJSON(null, errorMessage);
      }
      catch { }
    }
  }
}
