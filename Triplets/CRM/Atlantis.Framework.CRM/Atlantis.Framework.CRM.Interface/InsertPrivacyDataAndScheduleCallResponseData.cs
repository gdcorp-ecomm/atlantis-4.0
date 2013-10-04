using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CRM.Interface
{

  public enum ResultStatus
  {
    Success = 0,
    InvalidPhone = 1,
    OtherError = 2
  }

  public class InsertPrivacyDataAndScheduleCallResponseData : IResponseData
  {

    public ResultStatus Status { get; private set; }
    
    public static InsertPrivacyDataAndScheduleCallResponseData FromXml(string xml)
    {
      var result = new InsertPrivacyDataAndScheduleCallResponseData();

      if (xml.Contains("Success"))
      {
        result.Status = ResultStatus.Success;
      }
      else if (xml.Contains("InvalidPhone"))
      {
        result.Status = ResultStatus.InvalidPhone;
      }
      else
      {
        result.Status = ResultStatus.OtherError;
      }
      return result;
    }

    private InsertPrivacyDataAndScheduleCallResponseData()
    { 
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
