using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class RegionTests
  {
    const int _REGIONREQUESTTYPE = 666;

  }
}
