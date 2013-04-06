using System;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace Atlantis.Framework.Testing.MockHttpContext
{
  public static class MockHttpContext
  {
    [Obsolete("Please create your own MockHttpRequest and use SetFromWorkerRequest")]
    public static void SetMockHttpContext(string page, string url, string queryString)
    {
      UriBuilder builder = new UriBuilder(url);
      builder.Query = queryString;
      MockHttpRequest request = new MockHttpRequest(builder.Uri);
      SetFromWorkerRequest(request);
    }

    public static void SetFromWorkerRequest(HttpWorkerRequest mockRequest)
    {
      HttpContext mockContext = new HttpContext(mockRequest);

      ConstructorInfo[] consInfo = typeof(HttpSessionState).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
      object[] args = new object[1];
      args[0] = new MockSession();
      object oSession = consInfo[0].Invoke(args);
      HttpSessionState session = (HttpSessionState)oSession;
      mockContext.Items["AspSession"] = session;

      HttpContext.Current = mockContext;
    }

    public static void SetUser()
    {
      var windowsIdentity = WindowsIdentity.GetCurrent();
      if (windowsIdentity != null && windowsIdentity.AuthenticationType != null)
      {
        HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(windowsIdentity.Name, windowsIdentity.AuthenticationType), new string[0]);  
      }
    }
  }
}
