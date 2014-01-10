using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Interface;
using Atlantis.Framework.Iris.Interface.Objects;
using Atlantis.Framework.Providers.Iris.Interface;
using System.Threading;

namespace Atlantis.Framework.Providers.Iris
{
    public class IrisProvider : ProviderBase, IIrisProvider
    {
      public IrisProvider(IProviderContainer container) : base(container)
      {
      }

      public long QuickCreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress,
        string ipAddress, string createdBy, int privateLableId, string shopperId)
      {
        long retValue = -1;
        try
        {
          var requestData = new QuickCreateIncidentRequestData(subscriberId, subject, note, customerEmailAddress,
            ipAddress, createdBy, privateLableId, shopperId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisQuickCreate) as QuickCreateIncidentResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.IncidentId;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.QuickCreateIrisIncident", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

      public Incident CreateIrisIncident(int subscriberId, string subject, string note, string customerEmailAddress, string ipAddress,
        string createdBy, int privateLableId, int groupId, int serviceId, string shopperId)
      {
        Incident createdIncident = null;
        long incidentId;
        DateTime requestDateTime = DateTime.Now;
        try
        {
          var requestData = new CreateIncidentRequestData(subscriberId, subject, note, customerEmailAddress,
            ipAddress, groupId, serviceId, createdBy, privateLableId, shopperId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisCreate) as CreateIncidentResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            incidentId = responseData.IncidentId;
            if (incidentId != 0)
            {
              createdIncident = FetchNewIncidentWithRetry(shopperId, incidentId, requestDateTime, 5, 500);
            }
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.CreateIrisIncident", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return createdIncident;
      }

      private Incident FetchNewIncidentWithRetry(string shopperId, long incidentId, DateTime since, int numRetries, int intervalBetweenRetriesMillis)
      {
        Incident incident =  FetchNewIncident(shopperId, incidentId, since);
        if (incident == null)
        {
          int retries = 0;
          do
          {
            Thread.Sleep(500);
            incident = FetchNewIncident(shopperId, incidentId, since);
            retries++;
          } while (incident == null && retries < numRetries);
        }
        return incident;
      }

      private Incident FetchNewIncident(string shopperId, long incidentId, DateTime since)
      {
        Incident newIncident = null;
        // I don't see a method for grabbing a specific incident by ID
        // So, for now, we'll just grab for a very recent timeframe, and step through the return list and match ID - mjw 1/9/14
        var incidents = GetIncidents(shopperId, since.AddMinutes(-2), DateTime.Now.AddMinutes(2), true);

        foreach (Incident incident in incidents)
        {
          if (incident.IncidentId.Equals(incidentId))
          {
            newIncident = incident;
            break;
          }
        }
        return newIncident;
      }

      public Note AddIncidentNote(long incidentId, string note, string loginId)
      {
        Note newNote = null;
        long noteId = 0;
        try
        {
          var requestData = new AddIncidentNoteRequestData(incidentId, note, loginId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisAddNote) as AddIncidentNoteResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            noteId = responseData.IncidentNoteId;
            if (responseData.IsSuccess)
            {
              newNote = fetchNewNoteWithRetry(incidentId, noteId, 2, 300);
            }
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.AddIncidentNote", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return newNote;
      }

      private Note fetchNewNoteWithRetry( long incidentId, long noteId, int numRetries, int intervalBetweenRetriesMillis)
      {
        Note note = fetchNewNote(incidentId, noteId);
        if (note == null)
        {
          int retries = 0;
          do
          {
            Thread.Sleep(intervalBetweenRetriesMillis);
            note = fetchNewNote(incidentId, noteId);
            retries++;
          } while (note == null && retries < numRetries);
        }
        return note;
      }

      private Note fetchNewNote( long incidentId, long noteId)
      {
        Note newNote = null;
        List<Note> notes = GetIncidentNotes(incidentId, noteId - 1);
        if (notes != null)
        {
          foreach (Note note in notes)
          {
            if (note.IncidentNoteId.Equals(noteId))
            {
              newNote = note;
              break;
            }
          }
        }
        return newNote;
      }

      public List<Incident> GetIncidents(string shopperId, DateTime startDate, DateTime endDate, bool deepLoad)
      {
        var retValue = new List<Incident>();

        try
        {
          var requestData = new GetIncidentsByShopperIdAndDateRangeRequestData(shopperId, startDate, endDate);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisGetIncidents) as GetIncidentsByShopperIdAndDateRangeResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.Incidents;

            if (deepLoad)
            {
              foreach (var incident in responseData.Incidents)
              {
                //1 denotes ALL notes for ticket should be retrieved
                incident.Notes = GetIncidentNotes(incident.IncidentId, 1);
              }
            }

          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.GetIncidents", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

      public List<Note> GetIncidentNotes(long incidentId, long noteId)
      {
        var retValue = new List<Note>();

        try
        {
          var requestData = new GetIncidentCustomerNotesRequestData(incidentId, noteId);

          var responseData =
            Engine.Engine.ProcessRequest(requestData, EngineRequests.IrisGetIncidentNotes) as GetIncidentCustomerNotesResponseData;

          if (responseData != null && responseData.IsSuccess)
          {
            retValue = responseData.Notes;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("IrisProvider.GetIncidentNotes", "0", ex.Message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

        return retValue;
      }

    }
}
