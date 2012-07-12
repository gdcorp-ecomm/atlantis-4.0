using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReceiptSurveyInsert.Interface
{
  public class ReceiptSurveyInsertRequestData : RequestData
  {
    public string CountryCode { get; private set; }
    public int SurveyTypeId { get; private set; }
    public int SurveySelectionPosition { get; private set; }

    public ReceiptSurveyInsertRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, 
      string countryCode, int surveyTypeId, int surveySelectionPosition) :
      base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CountryCode = countryCode;
      SurveyTypeId = surveyTypeId;
      SurveySelectionPosition = surveySelectionPosition;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
