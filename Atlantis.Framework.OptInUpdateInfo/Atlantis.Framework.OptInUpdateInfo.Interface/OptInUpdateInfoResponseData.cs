using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OptInUpdateInfo.Interface
{
  public class OptInUpdateInfoResponseData : IResponseData
  {
    readonly List<Exception> _exceptions;
    readonly Dictionary<string, bool> _saveResults = new Dictionary<string, bool>();
    readonly private RequestData _request;

    public OptInUpdateInfoResponseData(Dictionary<string, bool> results, List<Exception> problems)
    {
      _exceptions = problems;
      _saveResults = results;
    }

    public OptInUpdateInfoResponseData(RequestData request, List<Exception> problems)
    {
      _request = request;
      _exceptions = problems;
    }

    public OptInUpdateInfoResponseData(List<Exception> problems)
    {
      _exceptions = problems;
    }

    public bool IsSuccess
    {
      get
      {
        return (_exceptions.Count == 0);
      }
    }

    public bool IsPartialSubmission
    {
      get
      {
        bool isPartial = false;

        if (_exceptions != null && _request != null)
        {
          isPartial = (!IsSuccess &&
                   (_exceptions.Count > 0 && (_exceptions.Count <= ((OptInUpdateInfoRequestData)_request).OptIns.Count)));
        }

        if (_exceptions != null && _saveResults.Select(x => x.Value == true).Count() < _saveResults.Count)
        {
          isPartial = true;
        }

        return isPartial;
      }
    }

    public bool IsTotalFailure
    {
      get
      {
        bool isFlop = false;

        if (_exceptions != null && _request != null)
        {
          isFlop = !IsSuccess && (_exceptions.Count >= ((OptInUpdateInfoRequestData)_request).OptIns.Count);
        }

        return isFlop;
      }
    }

    #region Implementation of IResponseData

    public string ToXML()
    {
      var sb = new StringBuilder();

      sb.Append("<OptInUpdateInfoResponseData>");
      foreach (var item in _saveResults)
      {
        sb.Append(string.Format("<Status name=\"{0}\" value=\"{1}\">", item.Key, item.Value));
      }

      sb.Append("</OptInUpdateInfoResponseData>");
      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      AtlantisException val = null;

      if (_exceptions.Count > 0)
      {
        val = _exceptions.OfType<AtlantisException>().FirstOrDefault();
      }

      return val;
    }

    public List<Exception> GetExceptions()
    {
      return _exceptions;
    }

    #endregion
  }
}
