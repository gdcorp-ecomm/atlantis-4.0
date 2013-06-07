namespace Atlantis.Framework.Warmup.Fixtures
{
  public interface IWarmupFixture
  {
    void SetupWarmup(IWarmupSetup iws);
    void TeardownWarmup();
  }
}
