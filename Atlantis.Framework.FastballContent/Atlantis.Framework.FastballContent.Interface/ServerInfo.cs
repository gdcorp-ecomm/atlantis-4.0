using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballContent.Interface
{
  [DataContract]
  public class ServerInfo
  {
    #region Public Properties

    [DataMember(Name = "Server")]
    public string Name
    {
      get;
      set;
    }

    [DataMember]
    public string Url
    {
      get;
      set;
    }

    #endregion

  }
}
