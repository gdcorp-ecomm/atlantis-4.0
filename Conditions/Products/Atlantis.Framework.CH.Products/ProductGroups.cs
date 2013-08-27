using System.Collections.Generic;

namespace Atlantis.Framework.CH.Products
{
  public static class ProductGroups
  {
    private static readonly Dictionary<string, int> ProductGroupNames = new Dictionary<string, int>
      {
        {"webhosting", 1},
        {"domainforward", 3},
        {"email", 4},
        {"domaintransfer", 6},
        {"parkedpages", 7},
        {"masking", 8},
        {"privateregistrations", 9},
        {"alternatedomains", 10},
        {"goodasgold", 11},
        {"domainmonitor", 12},
        {"searchenginevisibility", 13},
        {"websitebuilder", 15},
        {"smtprelay", 16},
        {"merchantaccount", 18},
        {"bluerazormember", 19},
        {"limiteddomainsreserved", 20},
        {"emailmarketing", 21},
        {"ssl", 22},
        {"shoppingcart", 23},
        {"dedicated", 24},
        {"virtualdedicated", 25},
        {"faxthruemail", 26},
        {"adminandmisc", 27},
        {"instantreseller", 28},
        {"brandsearch", 29},
        {"domains", 30},
        {"auctions", 31},
        {"calendar", 32},
        {"superreseller", 33},
        {"apireseller", 34},
        {"domainnameappraisal", 35},
        {"businessregistration", 36},
        {"blog", 38},
        {"domainmonetization", 39},
        {"domainownership", 40},
        {"commonaddons", 41},
        {"certifieddomain", 42},
        {"assistedservice", 43},
        {"domainbrokerage", 45},
        {"giftcard", 46},
        {"apparel", 47},
        {"discountclub", 48},
        {"filefolder", 50},
        {"logo", 52},
        {"hostingconnect", 53},
        {"customdesign", 54},
        {"premiumlisting", 55},
        {"protectedreg", 56},
        {"university", 57},
        {"premiumdomains", 58},
        {"marketingservices", 60},
        {"parkingheader", 62},
        {"customdomainassetdev", 63},
        {"microsofthostedexchange", 64},
        {"codesigningcertificates", 65},
        {"contactmanager", 66},
        {"businesscards", 67},
        {"letterhead", 68},
        {"webbannerads", 69},
        {"favicon", 71},
        {"flashcustomheader", 72},
        {"resellerheader", 73},
        {"onlinesurveys", 74},
        {"webstore", 75},
        {"incorporationservices", 78},
        {"admanager", 79},
        {"socialvisibility", 80},
        {"acceleratorbundle", 81},
        {"websiteprotection", 83},
        {"adspace", 84},
        {"easydb", 87},
        {"premiumdns", 89},
        {"datacenterondemand", 93},
        {"managedhosting", 94},
        {"costcobundle", 95},
        {"quickcontent", 97},
        {"accounting", 454}
      };

    public static bool TryGetProductId(string productGroupName, out int productGroupId)
    {
      return ProductGroupNames.TryGetValue(productGroupName.ToLowerInvariant(), out productGroupId);
    }
  }
}


