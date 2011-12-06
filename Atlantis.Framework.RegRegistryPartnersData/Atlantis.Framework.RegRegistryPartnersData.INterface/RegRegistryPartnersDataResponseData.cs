using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegRegistryPartnersData.Interface
{
  public class RegRegistryPartnersDataResponseData : IResponseData
  {
    private string _responseXML;
    private AtlantisException _atlException;

    public RegRegistryPartnersDataResponseData(string responseXML)
    {
      _responseXML = responseXML;
      _isSuccess = ParseResponseXml();
    }

    public RegRegistryPartnersDataResponseData(AtlantisException exAtlantis)
    {
      _responseXML = "";
      _atlException = exAtlantis;
      _isSuccess = ParseResponseXml();
    }

    public RegRegistryPartnersDataResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXML = sResponseXML;
      _atlException = new AtlantisException(oRequestData, "RegRegistryPartnersDataResponseData", ex.Message, string.Empty);
      _isSuccess = ParseResponseXml();
    }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    private List<TldRegistryData> _tldRegistryData;
    public List<TldRegistryData> TldRegistryData
    {
      get
      {
        return _tldRegistryData;
      }
    }

    private int _timeToLiveSeconds = 0;
    public int TimeToLiveSeconds
    {
      get
      {
        return _timeToLiveSeconds;
      }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
      get
      {
        return _errorMessage;
      }
    }

    #region Private Methods

    private bool ParseResponseXml()
    {
      bool success = false;

      if (!string.IsNullOrEmpty(_responseXML))
      {
        try
        {
          XmlDocument responseDoc = new XmlDocument();
          responseDoc.LoadXml(_responseXML);

          XmlNode successNode = responseDoc.SelectSingleNode("//RESULTS/SUCCESS");
          if (successNode != null)
          {
            success = ((successNode.InnerText == "1") || (successNode.InnerText == "true"));
          }

          if (success)
          {
            XmlNode tldsNode = responseDoc.SelectSingleNode("//RESULTS/TLDS");

            if (tldsNode != null)
            {
              XmlAttribute ttlAttr = (XmlAttribute)tldsNode.Attributes.GetNamedItem("TimeToLiveSeconds");
              if (ttlAttr != null)
              {
                int ttl;
                if (int.TryParse(ttlAttr.Value, out ttl))
                {
                  _timeToLiveSeconds = ttl;
                }
              }

              _tldRegistryData = new List<TldRegistryData>();

              XmlNodeList tldNodes = responseDoc.SelectNodes("//RESULTS/TLDS/TLD");
              foreach (XmlNode tld in tldNodes)
              {
                int rank = 0;
                string tldId = string.Empty, tldName = string.Empty, tldBidSnapshotId = string.Empty;

                foreach (XmlAttribute attr in tld.Attributes)
                {
                  switch (attr.Name)
                  {
                    case "TldId":
                      tldId = attr.Value;
                      break;
                    case "TldName":
                      tldName = attr.Value;
                      break;
                    case "Rank":
                      {
                        int tempRank;
                        if (int.TryParse(attr.Value, out tempRank))
                        {
                          rank = tempRank;
                        }
                      }
                      break;
                    case "TldBidSnapshotId":
                      tldBidSnapshotId = attr.Value;
                      break;
                  }
                }
                if (!string.IsNullOrEmpty(tldId) && !string.IsNullOrEmpty(tldName) && rank != 0 && !string.IsNullOrEmpty(tldBidSnapshotId))
                {
                  _tldRegistryData.Add(new TldRegistryData(tldId, tldName, rank, tldBidSnapshotId));
                }
              }

              success = ((_timeToLiveSeconds != 0) && (_tldRegistryData.Count > 0));
            }
          }
          else
          {
            XmlNode errMsgNode = responseDoc.SelectSingleNode("//RESULTS/ERROR/MESSAGE");
            _errorMessage = errMsgNode.Value;
          }
        }
        catch (Exception ex)
        {
          _errorMessage = ex.Message;
        }
      }

      return success;
    }

    #endregion

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return _responseXML;
    }

    #endregion
  }
}
