using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OptIn.Interface.Enums;

namespace Atlantis.Framework.OptInGetInfo.Interface
{
  public class OptInGetInfoRequestData : RequestData
  {
    private List<int> _optInPreferences = new List<int>();
    public string EmailAddress { get; set; }

    public OptInGetInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLableId, string email, List<OptInPublicationTypes> optInPreferences)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      foreach (OptInPublicationTypes item in optInPreferences)
      {
        _optInPreferences.Add((int)item);
      }

      EmailAddress = email;
      PrivateLabelId = privateLableId;
    }

    public OptInGetInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLableId, string email, List<int> optInPreferences)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {  
        _optInPreferences = optInPreferences;
        EmailAddress = email;
        PrivateLabelId = privateLableId;
    }

    public int PrivateLabelId { get; set; }

    public List<int> OptIns
    {
      get { return _optInPreferences; }
    }

    private TimeSpan _webServiceTimeout = new TimeSpan(0,0,10);
    public TimeSpan WebServiceTimeout
    {
      get { return _webServiceTimeout; }
      set
      {
          _webServiceTimeout = value;
      }
    }

    private TimeSpan _databaseTimeout = new TimeSpan(0, 0, 5);
    public TimeSpan DatabaseTimeout
    {
      get { return _databaseTimeout; }
      set
      {
        _databaseTimeout = value;
      }
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("OptInGetInfoRequests is not a cachable request.");
    }

    #endregion

    public void AddOptInOption(OptInPublicationTypes requestedType)
    {
      if (!_optInPreferences.Contains((int)requestedType))
      {
        _optInPreferences.Add((int)requestedType);
      }
    }
  }
}
