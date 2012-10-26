using System;

namespace Atlantis.Framework.Web.DynamicRouteHandler
{
  [Flags]
  public enum HttpRequestMethodType
  {
    Unknown = 1,
    Get = 2,
    Post = 4,
    Put = 8,
    Delete = 16
  }
}
