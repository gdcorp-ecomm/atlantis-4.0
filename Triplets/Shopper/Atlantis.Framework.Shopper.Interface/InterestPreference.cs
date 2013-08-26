namespace Atlantis.Framework.Shopper.Interface
{
  public class InterestPreference
  {
    public int InterestTypeId { get; private set; }
    public int CommunicationTypeId { get; private set; }

    public InterestPreference(int interestTypeId, int communicationTypeId)
    {
      InterestTypeId = interestTypeId;
      CommunicationTypeId = communicationTypeId;
    }
  }
}
