using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Impl.irisService;
using System;

namespace Atlantis.Framework.Iris.Impl
{
  public static class ServiceHelpers
  {
    public static IrisWebService GetServiceReference(string wsUrl)
    {
      return new IrisWebService() { Url = wsUrl };
    }
  }
}
