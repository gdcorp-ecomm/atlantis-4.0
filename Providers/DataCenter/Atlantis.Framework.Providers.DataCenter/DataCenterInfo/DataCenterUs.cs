namespace Atlantis.Framework.Providers.DataCenter.DataCenterInfo
{
  internal class DataCenterUs : IDataCenterInfo
  {
    public string Code
    {
      get { return "us"; }
    }

    public bool IsValidForCountryCode(string countryCode)
    {
      return true;
    }
  }
}
