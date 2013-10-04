using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CRM.Interface
{
  public class InsertPrivacyDataAndScheduleCallRequestData : RequestData
  {
    public string PrivacyDataXML { get; private set; }
    public string ScheduleXML { get; private set; }
    public int ClientId { get; private set; }
    
    public InsertPrivacyDataAndScheduleCallRequestData(string privacyDataXML, string scheduleXML, int clientId)
    {
      PrivacyDataXML = (privacyDataXML != null) ? privacyDataXML : string.Empty;
      ScheduleXML = (scheduleXML != null) ? scheduleXML : string.Empty;
      ClientId = clientId;
    }
  }
}
