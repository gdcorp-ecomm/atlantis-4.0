using System.Runtime.Serialization;

namespace Atlantis.Framework.Ecc.Interface.jsonHelpers
{
  [DataContract(Name = "EccJsonFault")]
  public class EccJsonFault
  {
    [DataMember(Name = "jsoap_fault")]
    private string JsoapFault { get; set; }
    public bool IsJsoapFault
    {
      get
      {
        bool isFault;
        bool.TryParse(JsoapFault, out isFault);
        return isFault;
      }
    }

    [DataMember(Name = "code")]
    public int ResultCode { get; set; }

    [DataMember(Name = "message")]
    public string Message { get; set; }

    [DataMember(Name = "actor")]
    public string Actor { get; set; }

    [DataMember(Name = "detail")]
    public string Detail { get; set; }
  }
}
