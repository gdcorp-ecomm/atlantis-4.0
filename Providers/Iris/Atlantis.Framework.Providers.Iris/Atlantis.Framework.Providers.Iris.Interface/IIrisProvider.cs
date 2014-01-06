using System;
using Atlantis.Framework.Iris.Interface.Objects;

namespace Atlantis.Framework.Providers.Iris.Interface
{
    public interface IIrisProvider
    {
      long QuickCreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress, string ipAddress, string createdBy, int privateLableId, string shopperId);
      long CreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress, string ipAddress, string createdBy, int privateLableId, int groupId, int serviceId, string shopperId);
      long AddIncidentNote(long incidentId, string note, string loginId);
      IncidentsList GetIncidents(string shopperId, DateTime startDate, DateTime endDate, bool deepLoad);
      NotesList GetIncidentNotes(long incidentId, int noteId);
    }
}
