
namespace Atlantis.Framework.LogDomainSearchResults.Interface
{
  internal class SuggestedDomain
  {
    public string Domain { get; set; }
    public int RegistrationType { get; set; }
    public int Area { get; set; }
    public int Position { get; set; }
    public string Price { get; set; }
    public int IsPriceDisplayed { get; set; }
    public int SpunName { get; set; }
    public int Availability { get; set; }

    public SuggestedDomain(string domain, int registrationType, int area, int position, string price, int isPriceDisplayed, int spunName, int availability)
    {
      Domain = domain;
      RegistrationType = registrationType;
      Area = area;
      Position = position;
      Price = price;
      IsPriceDisplayed = isPriceDisplayed;
      SpunName = spunName;
      Availability = availability;
    }

  }
}
