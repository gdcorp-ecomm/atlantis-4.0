using System.Runtime.Serialization;
using Atlantis.Framework.HDVD.Interface.Interfaces;

namespace Atlantis.Framework.HDVD.Interface
{
  [DataContract]
  public class HDVDHostingResponse : IHDVDHostingResponse
  {
    [DataMember]
    public int StatusCode { get; set; }
    
    [DataMember]
    public string Message { get; set; }

    [DataMember]
    public string Status { get; set; }
  }
}
