using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Iris.Interface
{
  public class CreateIncidentRequestData : RequestData
  {
    public CreateIncidentRequestData(int subscriberId,
                                string subject,
                                string note,
                                string customerEmail,
                                string ipAddress,
                                int groupId,
                                int servieId,
                                string createdBy,
                                int privateLabelId,
                                string shopperId)
    {
      SubscriberId = subscriberId;
      Subject = subject;
      Note = note;
      CustomerEmailAddress = customerEmail;
      IpAddress = ipAddress;
      CreatedBy = createdBy;
      PrivateLableId = privateLabelId;
      ShopperID = shopperId;
      GroupId = groupId;
      ServiceId = servieId;
    }

    public int SubscriberId { get; set; }
    public string Subject { get; set; }
    public string Note { get; set; }
    public string CustomerEmailAddress { get; set; }
    public string IpAddress { get; set; }
    public string CreatedBy { get; set; }
    public int PrivateLableId { get; set; }
    public int GroupId { get; set; }
    public int ServiceId { get; set; }

    public string ShopperId
    {
      get { return base.ShopperID; }
      set { base.ShopperID = value; }
    }
  }
}
