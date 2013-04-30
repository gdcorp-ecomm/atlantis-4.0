namespace Atlantis.Framework.Providers.SplitTesting.Interface
{
  public interface ISplitTestingProvider
  {
    string GetSplitTestingSide(int splitTestId);
  }
}
