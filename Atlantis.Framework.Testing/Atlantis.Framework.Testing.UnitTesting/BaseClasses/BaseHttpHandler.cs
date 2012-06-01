using System.Web;
using System.Web.SessionState;

namespace Atlantis.Framework.Testing.UnitTesting.BaseClasses
{
  public abstract class BaseHttpHandler : IHttpHandler, IRequiresSessionState
  {
    public bool IsReusable
    {
      get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
      ProcessRequest(new HttpContextWrapper(context));
    }

    public abstract void ProcessRequest(HttpContextBase context);
  }
}
