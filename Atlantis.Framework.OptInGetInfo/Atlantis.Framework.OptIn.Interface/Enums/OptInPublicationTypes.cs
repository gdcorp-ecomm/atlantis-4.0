namespace Atlantis.Framework.OptIn.Interface.Enums
{
  //Up until 100, database related fields
  //100 - 200, OptIns based on Shopper data
  //200 - 300, Specialized Opt Ins (bob's blog, etc.)
  //300+ Additional Service Opt ins
  public enum OptInPublicationTypes
  {
    Unknown= -1,
    Entertainment = 4,
    Tdnam = 8,
    MonthlyStatement = 39,
    BusinessOffers = 101,
    SmsCommunications = 102,
    NonPromotional = 103,
    RelatedOffers = 105,
    PostalCommunications = 106,
    PhoneCommunications = 107,
    BobsBlog = 200, 
    WhoIsMailer = 300
  }

  public enum EmailFormats
  {
    Text = 1,
    Html = 2
  }
}

