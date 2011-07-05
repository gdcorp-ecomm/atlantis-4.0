using System.Web;
using System.IO;
using System.Reflection;
using System.Web.SessionState;

namespace Atlantis.Framework.Testing.MockHttpContext
{
  public static class MockHttpContext
  {
    public static void SetMockHttpContext(string page, string url, string queryString)
    {
      StringWriter writer = new StringWriter();
      HttpRequest request = new HttpRequest(page, url, queryString);
      HttpResponse response = new HttpResponse(writer);
      HttpContext mockContext = new System.Web.HttpContext(request, response);

      ConstructorInfo[] consInfo = typeof(HttpSessionState).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
      object[] args = new object[1];
      args[0] = new MockSession();
      object oSession = consInfo[0].Invoke(args);
      HttpSessionState session = (HttpSessionState)oSession;
      mockContext.Items["AspSession"] = session;

      HttpContext.Current = mockContext;
    }
  }
}
