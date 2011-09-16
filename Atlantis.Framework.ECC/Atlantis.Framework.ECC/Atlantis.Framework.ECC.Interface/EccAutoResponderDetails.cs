using System.Runtime.Serialization;

namespace Atlantis.Framework.Ecc.Interface
{
  [DataContract]
  public class EccAutoResponderDetails
  {
    private const int ACTIVE = 1;

    [DataMember(Name = "ar_status")]
    private string ar_status { get; set; }
    public int Status
    {
      get
      {
        int parse;
        int.TryParse(ar_status, out parse);
        if (!int.TryParse(ar_status, out parse))
        {
          parse = 0;
        }

        return parse;
      }
      set { ar_status = value.ToString(); }
    }

    [DataMember(Name = "ar_message")]
    public string Message { get; set; }

    [DataMember(Name = "ar_subject")]
    public string Subject { get; set; }

    [DataMember(Name = "ar_from")]
    public string From { get; set; }

    [DataMember(Name = "ar_start")]
    public string StartDate { get; set; }

    [DataMember(Name = "ar_end")]
    public string EndDate { get; set; }

    [DataMember(Name = "ar_single")]
    private string ar_single { get; set; }
    public int SingleResponse
    {
      get
      {
        int parse;
        int.TryParse(ar_single, out parse);
        if (!int.TryParse(ar_single, out parse))
        {
          parse = 0;
        }

        return parse;
      }
      set { ar_single = value.ToString(); }
    }

    private bool? _isActive;
    public bool IsActive
    {
      get
      {
        if(_isActive == null)
        {
          _isActive = Status == ACTIVE;
        }

        return _isActive.Value;
      }
    }

    private bool? _isSingleResponse;
    public bool IsSingleResponse
    {
      get
      {
        if (_isSingleResponse == null)
        {
          _isSingleResponse = Status == ACTIVE;
        }

        return _isSingleResponse.Value;
      }
    }
  }
}
