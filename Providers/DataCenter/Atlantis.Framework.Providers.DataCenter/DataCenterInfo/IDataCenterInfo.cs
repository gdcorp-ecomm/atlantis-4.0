namespace Atlantis.Framework.Providers.DataCenter.DataCenterInfo
{
  internal interface IDataCenterInfo
  {
    string Code { get; }
    bool IsValidForCountryCode(string countryCode);
  }
}
