using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDPhaseDateListResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _xmlData;

    private readonly List<TLDPhaseDate> _tldPhaseDates;

    public IEnumerable<TLDPhaseDate> TldPhaseDates
    {
      get { return _tldPhaseDates; }
    }

    public static TLDPhaseDateListResponseData FromException(RequestData requestData, Exception ex)
    {
      return new TLDPhaseDateListResponseData(requestData, ex);
    }

    private TLDPhaseDateListResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "TLDPhaseDateListResponseData.ctor", message, inputData);
    }

    public static TLDPhaseDateListResponseData FromDataCacheElement(XElement dataCacheElement)
    {
      return new TLDPhaseDateListResponseData(dataCacheElement);
    }

    private TLDPhaseDateListResponseData(XElement dataCacheElement)
    {
      _xmlData = dataCacheElement.ToString();
      _tldPhaseDates = new List<TLDPhaseDate>();

      foreach (XElement itemElement in dataCacheElement.Elements("item"))
      {
        try
        {
          var strPhaseCode = itemElement.Attribute("gdshop_tldPhase").Value;
          var strPhaseStartDate = itemElement.Attribute("phaseStartDate").Value;
          var strPhaseEndDate = itemElement.Attribute("phaseEndDate").Value;

          DateTime phaseStartDate;
          DateTime.TryParse(strPhaseStartDate, out phaseStartDate);

          DateTime phaseEndDate;
          DateTime.TryParse(strPhaseEndDate, out phaseEndDate);

          var tldPhaseDate = new TLDPhaseDate(strPhaseCode, phaseStartDate, phaseEndDate);
          _tldPhaseDates.Add(tldPhaseDate);
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var aex = new AtlantisException("TLDPhaseDateListResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_xmlData != null)
      {
        result = _xmlData;
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
