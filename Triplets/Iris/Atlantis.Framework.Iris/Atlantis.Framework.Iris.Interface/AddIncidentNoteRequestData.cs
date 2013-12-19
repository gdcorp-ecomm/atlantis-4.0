using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Iris.Interface
{
  public class AddIncidentNoteRequestData : RequestData
  {
    public AddIncidentNoteRequestData(long incidentId, string note, string loginId )
    {
      IncidentId = incidentId;
      Note = note;
      LoginId = loginId;
    }

    public long IncidentId { get; set; }
    public string Note { get; set; }
    public string LoginId { get; set; }
  }
}
