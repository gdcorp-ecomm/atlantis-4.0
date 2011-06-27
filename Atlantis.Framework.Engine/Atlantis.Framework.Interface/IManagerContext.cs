using System.Collections.Specialized;

namespace Atlantis.Framework.Interface
{
  public interface IManagerContext
  {
    bool IsManager { get; }
    string ManagerUserId { get; }
    string ManagerUserName { get; }
    NameValueCollection ManagerQuery { get; }
    string ManagerShopperId { get; }
    int ManagerPrivateLabelId { get; }
    int ManagerContextId { get; }
  }
}
