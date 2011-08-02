using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketItemCounts.Interface
{
  public class GetBasketItemCountsResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _responseText;
    private List<int> _itemCounts;

    public int GetItemCount(int basketTypeIndex)
    {
      int result = 0;
      if ((_itemCounts.Count > basketTypeIndex) && (basketTypeIndex >= 0))
      {
        result = _itemCounts[basketTypeIndex];
      }
      return result;
    }

    private void ProcessResponseString(string responseString)
    {
      if (!string.IsNullOrEmpty(responseString))
      {
        string[] countStrings = responseString.Split('|');
        _itemCounts = new List<int>(countStrings.Length);
        foreach (string countString in countStrings)
        {
          int count;
          if (int.TryParse(countString, out count))
          {
            _itemCounts.Add(count);
          }
        }
      }
      else
      {
        _itemCounts = new List<int>();
      }
    }

    public GetBasketItemCountsResponseData(string responseText)
    {
      _responseText = responseText;
      ProcessResponseString(responseText);
    }

    public GetBasketItemCountsResponseData(string responseText, AtlantisException exAtlantis)
    {
      _responseText = responseText;
      _exception = exAtlantis;
      ProcessResponseString(null);
    }
    public GetBasketItemCountsResponseData(string responseText, RequestData oRequestData, Exception ex)
    {
      _responseText = responseText;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
      ProcessResponseString(null);
    }

    public bool IsSuccess
    {
      get
      {
        return (_exception == null);
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return "<ItemCounts>" + _responseText + "</ItemCounts>";
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
