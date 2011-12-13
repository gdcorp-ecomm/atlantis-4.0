using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReceiptSurvey.Interface
{
  public class ReceiptSurveyRequestData : RequestData
  {

    #region Properties

    private readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(20);

    private string _howheard=string.Empty;
    public string HowHeard
    {
      get
      {
        return _howheard;
      }
      set
      {
        _howheard = value;
      }
    }

    private int _display_position=0;
    public int DisplayPosition
    {
      get
      {
        return _display_position;
      }
      set
      {
        _display_position = value;
      }
    }

    private int _response_section = 0;
    public int ResponseSection
    {
      get
      {
        return _response_section;
      }
      set
      {
        _response_section = value;
      }
    }

    public int SurveyTypeID { get; set; }


    #endregion

    #region Constructors

    public ReceiptSurveyRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string howHeard,
                                  int display_position,
                                  int survey_typeId,
                                  int response_section,
                                  TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      _howheard = howHeard;
      _display_position = display_position;
      _response_section = response_section;
      SurveyTypeID=survey_typeId;
      RequestTimeout = requestTimeout;
    }

    public ReceiptSurveyRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderIo,
                                  string pathway,
                                  int pageCount,
                                  string howHeard,
                                  int display_position,
                                  int survey_typeId,
                                  int response_section)
      : base(shopperId, sourceUrl, orderIo, pathway, pageCount)
    {
      _howheard = howHeard;
      _display_position = display_position;
      _response_section = response_section;
      SurveyTypeID = survey_typeId;
    }   
    #endregion

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ReceiptSurveyRequestData");     
    }


  }
}
