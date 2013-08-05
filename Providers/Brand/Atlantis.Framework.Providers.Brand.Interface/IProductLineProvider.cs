namespace Atlantis.Framework.Providers.Brand.Interface
{
  public interface IProductLineProvider
  {
    string GetProductLineName(string productLineKey, int overrideDefault = 0);
  }
}
