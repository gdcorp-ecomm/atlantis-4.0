
namespace Atlantis.Framework.SessionCache
{
  public interface ISessionSerializableResponse
  {
    string SerializeSessionData();
    void DeserializeSessionData(string sessionData);
  }
}
