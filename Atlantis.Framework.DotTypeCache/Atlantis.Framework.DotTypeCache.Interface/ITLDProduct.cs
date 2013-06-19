namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDProduct
  {
    ITLDValidYearsSet RegistrationYears { get; }
    ITLDValidYearsSet TransferYears { get; }
    ITLDValidYearsSet RenewalYears { get; }
    ITLDValidYearsSet ExpiredAuctionsYears { get; }
    ITLDValidYearsSet PreregistrationYears(string type);
    bool HasPreRegApplicationFee(string type);
  }
}
