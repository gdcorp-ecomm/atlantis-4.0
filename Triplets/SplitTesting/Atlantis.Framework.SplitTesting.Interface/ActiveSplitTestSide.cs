using Atlantis.Framework.Providers.SplitTesting.Interface;

namespace Atlantis.Framework.SplitTesting.Interface
{
  public class ActiveSplitTestSide : IActiveSplitTestSide
  {
    public int SideId { get; set; }
    public string Name { get; set; }
    public double Allocation { get; set; }
  }
}
