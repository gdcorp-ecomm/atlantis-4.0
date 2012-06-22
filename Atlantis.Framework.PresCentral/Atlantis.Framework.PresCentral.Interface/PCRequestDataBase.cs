using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public abstract class PCRequestDataBase : RequestData
  {
    public abstract IResponseData CreateResponse(PCResponse responseData);
    public abstract IResponseData CreateResponse(AtlantisException ex);

    public virtual bool AllowErrorResponses
    {
      get { return false; }
    }

    NameValueCollection _parameters = new NameValueCollection();

    public PCRequestDataBase(string shopperId, string sourceURL, string orderId, string pathway, int pageCount) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public void AddQueryParameter(string key, string value)
    {
      _parameters[key] = value;
    }

    public void AddQueryParameters(NameValueCollection queryItems)
    {
      if (queryItems != null)
      {
        foreach (string key in queryItems.Keys)
        {
          _parameters[key] = queryItems[key];
        }
      }
    }

    public string GetQuery()
    {
      List<string> queryItems = new List<string>(_parameters.Count);

      foreach (string key in _parameters.Keys)
      {
        if ((!string.IsNullOrEmpty(key)) && (_parameters[key] != null))
        {
          string queryItem = Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(_parameters[key]);
          queryItems.Add(queryItem);
        }
      }

      // To allow for consistent keys, sort the parameters
      queryItems.Sort();

      return string.Join("&", queryItems);
    }

    

  }
}
