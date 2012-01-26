using Atlantis.Framework.Interface;
using System.Collections.Generic;
using Atlantis.Framework.FastballGetOffersDetail.Interface;

namespace Atlantis.Framework.FastballGetOffersDetail.Impl
{
  public class FastballGetOffersDetailRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FastballGetOffersDetailRequestData offersRequest = requestData as FastballGetOffersDetailRequestData;
      if (offersRequest != null)
      {
        if (offersRequest.ReturnStubbedData)
        {
          return GetStubbedResponse();
        }
      }
      return null;
    }

    #region Stubbed data
    Dictionary<string, Product> Products { get; set; }
    private FastballGetOffersDetailResponse GetStubbedResponse()
    {
      GetHostingProducts();

      FastballGetOffersDetailResponse response = new FastballGetOffersDetailResponse()
      {
        IsSuccess = true,
        OfferDetailList = new List<OfferDetail>() { 
          GetUpsellOffer(),
          GetDomainSearchOffer(),
          GetHostingOffer(),
          GetWSTOffer()
        }
      };

      return response;
    }

    private void GetHostingProducts()
    {
      Products = new Dictionary<string, Product>() { 
        {"58", new Product() { Pfid="58", Qty=2, DisplayOrder=0, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="301", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="302", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="303", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           }
         }
        },
        {"6700", new Product() { Pfid="6700", Qty=1, DisplayOrder=1, IsDefault=true,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="304", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="305", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="306", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           }
         }
         },
        {"6701", new Product() { Pfid="6701", Qty=1, DisplayOrder=2, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="307", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="308", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="309", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           }
        }},
        {"6702", new Product() { Pfid="6702", Qty=1, DisplayOrder=3, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="310", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="311", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="312", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"64",                 new Product() { Pfid="64", Qty=2, DisplayOrder=4, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="313", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="314", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="315", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6710", new Product() { Pfid="6710", Qty=1, DisplayOrder=5, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="316", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="317", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="318", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6711", new Product() { Pfid="6711", Qty=1, DisplayOrder=6, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="319", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="320", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="321", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6712", new Product() { Pfid="6712", Qty=1, DisplayOrder=7, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="322", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="323", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="324", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},

        {"156",                new Product() { Pfid="156", Qty=1, DisplayOrder=0, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="325", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="326", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="327", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6730", new Product() { Pfid="6730", Qty=1, DisplayOrder=1, IsDefault=true,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="328", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="329", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="330", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6731", new Product() { Pfid="6731", Qty=1, DisplayOrder=2, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="331", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="332", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="333", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6732", new Product() { Pfid="6732", Qty=1, DisplayOrder=3, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="334", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="335", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="336", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"154",                new Product() { Pfid="154", Qty=2, DisplayOrder=4, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="337", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="338", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="339", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6720", new Product() { Pfid="6720", Qty=1, DisplayOrder=5, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="340", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="341", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="342", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6721", new Product() { Pfid="6721", Qty=1, DisplayOrder=6, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="343", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="344", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="345", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"6722", new Product() { Pfid="6722", Qty=1, DisplayOrder=7, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="346", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="347", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="348", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        
        {"7225", new Product() { Pfid="7225", Qty=1, DisplayOrder=0, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="349", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="350", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="351", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7227", new Product() { Pfid="7227", Qty=1, DisplayOrder=1, IsDefault=true,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="352", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="353", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="354", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7228", new Product() { Pfid="7228", Qty=1, DisplayOrder=2, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="355", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="356", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="357", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7229", new Product() { Pfid="7229", Qty=1, DisplayOrder=3, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="358", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="359", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="360", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7226", new Product() { Pfid="7226", Qty=2, DisplayOrder=4, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="361", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="362", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="363", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7232", new Product() { Pfid="7232", Qty=1, DisplayOrder=5, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="364", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="365", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="366", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7233", new Product() { Pfid="7233", Qty=1, DisplayOrder=6, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="367", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="368", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="369", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }},
        {"7234", new Product() { Pfid="7234", Qty=1, DisplayOrder=7, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="370", Type="Cart", TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2011, 01, 01)  },
             new PackageDetailLite() { Id="371", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 02, 01)  },
             new PackageDetailLite() { Id="372", Type="Cart", TargetDate = new System.DateTime(2012, 02, 01), EndDate = new System.DateTime(2012, 03, 01)  }
           } }}
      };
    }

    private void GetWSTProducts()
    {
      Products = new Dictionary<string, Product>() { 
        {"7521", new Product() { Pfid="7521", Qty=3, DisplayOrder=0, IsDefault=true,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="401", Type="Cart", TargetDate = new System.DateTime(201, 012, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7524", new Product() { Pfid="7524", Qty=1, DisplayOrder=1, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="402", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7525", new Product() { Pfid="7525", Qty=1, DisplayOrder=2, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="403", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7526", new Product() { Pfid="7526", Qty=1, DisplayOrder=3, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="404", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },


        {"7502", new Product() { Pfid="7502", Qty=1, DisplayOrder=0, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="405", Type="Cart", TargetDate = new System.DateTime(201, 012, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7509", new Product() { Pfid="7509", Qty=1, DisplayOrder=1, IsDefault=true,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="406", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7510", new Product() { Pfid="7510", Qty=1, DisplayOrder=2, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="407", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7511", new Product() { Pfid="7511", Qty=1, DisplayOrder=3, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="408", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },


        {"7503", new Product() { Pfid="7503", Qty=1, DisplayOrder=0, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="409", Type="Cart", TargetDate = new System.DateTime(201, 012, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7514", new Product() { Pfid="7514", Qty=1, DisplayOrder=1, IsDefault=true,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="410", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7515", new Product() { Pfid="7515", Qty=1, DisplayOrder=2, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="411", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        },
        {"7516", new Product() { Pfid="7516", Qty=1, DisplayOrder=3, IsDefault=false,
           PackagesList = new List<PackageDetailLite>() { 
             new PackageDetailLite() { Id="412", Type="Cart", TargetDate = new System.DateTime(2012, 01, 01), EndDate = new System.DateTime(2012, 12, 31)  },
           }
         }
        }
      };
    }

    private OfferDetail GetUpsellOffer()
    {
      OfferDetail offer = new OfferDetail() { fbiOfferId = "10110", ProductGroupCode = "215", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty,
           ProductGroupAttributes = null, //Product Group Data for Upsells?
           DataItems = new List<DataItem>() {
             new DataItem() { Id="FragmentImage", Type="text",
              Attributes = new Dictionary<string, List<string>>() {
                { "src", new List<string>() { "http://img1.wsimg.com/fos/base/1/67931_sprite_icn_xsell.png" } },
                { "height", new List<string>() { "39px" } },
                { "width", new List<string>() { "39px" } }
              }
             },

             new DataItem() { Id="bannerContent", Type="text",
              Attributes = new Dictionary<string, List<string>>() {
                { "header", new List<string>() { "Upgrade to Unlimited+ Email and never worry about your inbox being too full again!" } },
                { "productdescription", new List<string>() { "You already have 24 months of Deluxe Email for {{product:price:<pfid>:dropdecimal:monthly}}." } },
                { "productfeaturebullet1", new List<string>() { "Unlimited storage." } },
                { "productfeaturebullet2", new List<string>() { "10 email addresses, POP3 and IMAP configured." } },
                { "productfeaturebullet3", new List<string>() { "Includes economy Online File Folder® and 10-user Group Calendar FREE!" } },
                { "pricingDisplay", new List<string>() { "Sign me up for Unlimited Email Just {{price:monthly:keepdecimal}}." } },
              }
             },

             new DataItem() { Id="buttons", Type="text",
              Attributes = new Dictionary<string, List<string>>() {
                { "addCiCode", new List<string>() { "55517" } },
                { "inCartButtonText", new List<string>() { "In Cart!" } },
                { "removeCiCode", new List<string>() { "55518" } },
                { "standardButtonText", new List<string>() { "Add" } }
              }
             },

             new DataItem() { Id="format", Type="Nested",
              Attributes = new Dictionary<string, List<string>>() {
                { "format", new List<string>() { "XSModalUpsell" } }
              }
             }
           }
          };

      return offer;
    }

    private OfferDetail GetDomainSearchOffer()
    {
      OfferDetail offer = //Domain Search
          new OfferDetail() { fbiOfferId = "10111", ProductGroupCode = string.Empty, DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty,
           ProductGroupAttributes = null, //Product Group Data for Upsells?
           DataItems = new List<DataItem>() {
             new DataItem() { Id="format", Type="Nested",
              Attributes = new Dictionary<string, List<string>>() {
                { "format", new List<string>() { "XSModalDomainSearch" } }
              }
             }
           }
          };
      return offer;
    }

    private OfferDetail GetHostingOffer()
    {
      OfferDetail offer = 
          new OfferDetail() { fbiOfferId = "10112", ProductGroupCode = "216", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty,
             DataItems = new List<DataItem>() {
                //new DataItem() { Id="FragmentImage", Type="text",
                //    Attributes = new Dictionary<string, List<string>>() {
                //    { "src", new List<string>() { "http://img1.wsimg.com/fos/base/1/67931_sprite_icn_xsell.png" } },
                //    { "height", new List<string>() { "39px" } },
                //    { "width", new List<string>() { "39px" } },
                //    { "backgroundposition-x", new List<string>() { "0" } },
                //    { "backgroundposition-y", new List<string>() { "0" } },
                //  }
                //},

                new DataItem() { Id="bannerContent", Type="text",
                  Attributes = new Dictionary<string, List<string>>() {
                    { "header", new List<string>() { "Web Hosting" } },
                    { "productDescriptionLine1", new List<string>() { "Trust the world's largest hosting provider<sup>2</sup>" } },
                    { "productDescriptionLine2", new List<string>() { "to provide a secure, reliable home for your site." } },
                    { "pricingDisplay", new List<string>() { "As low as {{price:monthly:keepdecimal}}." } },  
                    { "DetailsLinkText", new List<string>() { "Details" } },
                    { "detailsHeader", new List<string>()  { "Our revolutionary 4GH&trade; Web Hosting features:" } },  
                    { "detailsBullet1", new List<string>() { "Free setup, no ad banners or pop-ups and no annual commitment." } },
                    { "detailsBullet2", new List<string>() { "Free software for photo galleries, blogs, forums and more." } },
                    { "detailsBullet3", new List<string>() { "Unlimited bandwidth so you never have to worry about overage fees." } },
                    { "detailsBullet4", new List<string>() { "99.9% guaranteed uptime, expert 24/7 support and MUCH more." } },
                    { "detailsBullet5", new List<string>() { string.Empty } },
                    { "detailsBullet6", new List<string>() { string.Empty } },
                  }
                 },

               new DataItem() { Id="buttons", Type="text",
                  Attributes = new Dictionary<string, List<string>>() {
                    { "addCiCode", new List<string>() { "55517" } },
                    { "inCartButtonText", new List<string>() { "In Cart!" } },
                    { "removeCiCode", new List<string>() { "55518" } },
                    { "standardButtonText", new List<string>() { "Add" } }
                  }
               },

               new DataItem() { Id="PrimaryAttribute", Type="text",
                  Attributes = new Dictionary<string, List<string>>() {
                    { "name", new List<string>() { "PlanLevel" } },
                  }
               },

               new DataItem() { Id="DropDownContent", Type="text",
                  Attributes = new Dictionary<string, List<string>>() {
                    { "D1_Features", new List<string>() { "{{productgroup:attribute:Storage}}&bull;{{productgroup:attribute:Bandwidth}}&bull;{{productgroup:attribute:EmailAccounts}}&bull;{{productgroup:attribute:MySQLDatabases}}" } },
                    { "D1_Free", new List<string>() { "{{productgroup:attribute:FreeFeatures}}" } },
                    { "D2_TermsAndPricing", new List<string>() { "{{productgroup:attribute:OperatingSystem}}:&amp;nbsp;{{productgroup:term:monthly}}:&amp;nbsp;{{productgroup:price}}:&amp;nbsp;{{productgroup:discount}}:&amp;nbsp;" } }
                  }
               }
             },
             ProductGroupAttributes = new List<ProductGroupAttrib>() {
               new ProductGroupAttrib() { Name = "PlanLevel",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="Economy",
                      Products = new List<Product>() { 
                        Products["58"],
                        Products["6700"],
                        Products["6701"],
                        Products["6702"],
                        Products["64"], 
                        Products["6710"],
                        Products["6711"],
                        Products["6712"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="Deluxe",
                      Products = new List<Product>() { 
                        Products["156"], 
                        Products["6730"],
                        Products["6731"],
                        Products["6732"],
                        Products["154"], 
                        Products["6720"],
                        Products["6721"],
                        Products["6722"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="Ultimate",
                      Products = new List<Product>() { 
                         Products["7225"],
                         Products["7227"],
                         Products["7228"],
                         Products["7229"],
                         Products["7226"],
                         Products["7232"],
                         Products["7233"],
                         Products["7234"]
                      }
                   },
                 }
               },

               new ProductGroupAttrib() { Name = "Storage",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="10 GB Space",
                      Products = new List<Product>() { 
                        Products["58"],
                        Products["6700"],
                        Products["6701"],
                        Products["6702"],
                        Products["64"], 
                        Products["6710"],
                        Products["6711"],
                        Products["6712"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="150 GB Space",
                      Products = new List<Product>() { 
                        Products["156"], 
                        Products["6730"],
                        Products["6731"],
                        Products["6732"],
                        Products["154"], 
                        Products["6720"],
                        Products["6721"],
                        Products["6722"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="Unlimited Space",
                      Products = new List<Product>() { 
                         Products["7225"],
                         Products["7227"],
                         Products["7228"],
                         Products["7229"],
                         Products["7226"],
                         Products["7232"],
                         Products["7233"],
                         Products["7234"]
                      }
                   },
                 }
               },

               
               new ProductGroupAttrib() { Name = "Bandwidth",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="Unlimited",
                      Products = new List<Product>() { 
                        Products["58"],
                        Products["6700"],
                        Products["6701"],
                        Products["6702"],
                        Products["64"], 
                        Products["6710"],
                        Products["6711"],
                        Products["6712"],
                        Products["156"], 
                        Products["6730"],
                        Products["6731"],
                        Products["6732"],
                        Products["154"], 
                        Products["6720"],
                        Products["6721"],
                        Products["6722"]
                      }
                   }
                 }
               },

               new ProductGroupAttrib() { Name = "EmailAccounts",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="100 Email Accounts",
                      Products = new List<Product>() { 
                        Products["58"],
                        Products["6700"],
                        Products["6701"],
                        Products["6702"],
                        Products["64"], 
                        Products["6710"],
                        Products["6711"],
                        Products["6712"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="500 Email Accounts",
                      Products = new List<Product>() { 
                        Products["156"], 
                        Products["6730"],
                        Products["6731"],
                        Products["6732"],
                        Products["154"], 
                        Products["6720"],
                        Products["6721"],
                        Products["6722"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="1000 Email Accounts",
                      Products = new List<Product>() { 
                         Products["7225"],
                         Products["7227"],
                         Products["7228"],
                         Products["7229"],
                         Products["7226"],
                         Products["7232"],
                         Products["7233"],
                         Products["7234"]
                      }
                   },
                 }
               },


               new ProductGroupAttrib() { Name = "MySQLDatabases",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="10 MySQL Databases",
                      Products = new List<Product>() { 
                        Products["58"],
                        Products["6700"],
                        Products["6701"],
                        Products["6702"],
                        Products["64"], 
                        Products["6710"],
                        Products["6711"],
                        Products["6712"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="25 MySQL Databases",
                      Products = new List<Product>() { 
                        Products["156"], 
                        Products["6730"],
                        Products["6731"],
                        Products["6732"],
                        Products["154"], 
                        Products["6720"],
                        Products["6721"],
                        Products["6722"]
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="Unlimited MySQL Databases",
                      Products = new List<Product>() { 
                         Products["7225"],
                         Products["7227"],
                         Products["7228"],
                         Products["7229"],
                         Products["7226"],
                         Products["7232"],
                         Products["7233"],
                         Products["7234"]
                      }
                   },
                 }
               },

               
               new ProductGroupAttrib() { Name = "FreeFeatures",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="Includes FREE<span class=\"sup\">††</span>Premium DNS, Site Scanner, SSL Certificate, and Fixed IP Address",
                      Products = new List<Product>() { 
                         Products["7225"],
                         Products["7227"],
                         Products["7228"],
                         Products["7229"],
                         Products["7226"],
                         Products["7232"],
                         Products["7233"],
                         Products["7234"]
                      }
                   }
                 }
               },

               new ProductGroupAttrib() { Name = "OperatingSystem",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="Linux",
                      Products = new List<Product>() { 
                        Products["58"], 
                        Products["6700"],
                        Products["6701"],
                        Products["6702"],
                        Products["156"], 
                        Products["6730"],
                        Products["6731"],
                        Products["6732"],
                        Products["7225"],
                        Products["7227"],
                        Products["7228"],
                        Products["7229"]
                      }
                   },

                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="Windows",
                      Products = new List<Product>() { 
                         Products["154"], 
                         Products["6720"],
                         Products["6721"],
                         Products["6722"],
                         Products["64"], 
                         Products["6710"],
                         Products["6711"],
                         Products["6712"],
                         Products["7226"],
                         Products["7232"],
                         Products["7233"],
                         Products["7234"]
                      }
                   }
                 }
               },
             },
          };

      return offer;
    }

    private OfferDetail GetWSTOffer()
    {
      OfferDetail offer = //Website Tonight
          new OfferDetail() { fbiOfferId = "10113", ProductGroupCode = "217", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty,
             DataItems = new List<DataItem>() {
                //new DataItem() { Id="FragmentImage", Type="text",
                //    Attributes = new Dictionary<string, List<string>>() {
                //    { "src", new List<string>() { "http://img1.wsimg.com/fos/base/1/67931_sprite_icn_xsell.png" } },
                //    { "height", new List<string>() { "39px" } },
                //    { "width", new List<string>() { "39px" } },
                //    { "backgroundposition-x", new List<string>() { "0" } },
                //    { "backgroundposition-y", new List<string>() { "0" } },
                //  }
                //},

                new DataItem() { Id="bannerContent", Type="text",
                  Attributes = new Dictionary<string, List<string>>() {
                    { "header", new List<string>() { "WebSite Tonight®" } },
                    { "productDescriptionLine1", new List<string>() { "Create your own unique website in minutes!" } },
                    { "productDescriptionLine2", new List<string>() { "No design or technical skills required." } },
                    { "pricingDisplay", new List<string>() { "As low as {{price:monthly:keepdecimal}}." } },  
                    { "DetailsLinkText", new List<string>() { "Details" } },
                    { "detailsHeader", new List<string>()  { "Our revolutionary 4GH&trade; Web Hosting features:" } },  
                    { "detailsBullet1", new List<string>() { "Free setup, no ad banners or pop-ups and no annual commitment." } },
                    { "detailsBullet2", new List<string>() { "Free software for photo galleries, blogs, forums and more." } },
                    { "detailsBullet3", new List<string>() { "Unlimited bandwidth so you never have to worry about overage fees." } },
                    { "detailsBullet4", new List<string>() { "99.9% guaranteed uptime, expert 24/7 support and MUCH more." } },
                    //{ "detailsBullet5", new List<string>() { string.Empty } },
                    //{ "detailsBullet6", new List<string>() { string.Empty } },
                  }
                 },

                 new DataItem() { Id="buttons", Type="text",
                  Attributes = new Dictionary<string, List<string>>() {
                    { "addCiCode", new List<string>() { "55519" } },
                    { "inCartButtonText", new List<string>() { "In Cart!" } },
                    { "removeCiCode", new List<string>() { "55520" } },
                    { "standardButtonText", new List<string>() { "Add" } }
                  }
                 },

                 new DataItem() { Id="PrimaryAttribute", Type="text",
                    Attributes = new Dictionary<string, List<string>>() {
                      { "name", new List<string>() { "PlanLevel" } },
                    }
                 },

                 new DataItem() { Id="DropDownContent", Type="text",
                    Attributes = new Dictionary<string, List<string>>() {
                      { "D1_Features", new List<string>() { "{{productgroup:attribute:Website}}&bull;{{productgroup:attribute:Diskspace}}&bull;{{productgroup:attribute:Bandwidth}}}}" } },
                      { "D2_TermsAndPricing", new List<string>() { "{{{{productgroup:term:monthly}} mos:nbsp;{{productgroup:price}}:&amp;nbsp;{{productgroup:discount}}:&amp;nbsp;" } }
                    }
                 }


             },
             ProductGroupAttributes = new List<ProductGroupAttrib>() {
               new ProductGroupAttrib() { Name = "Planlevel",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                   new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="Economy",
                      Products = new List<Product>() { 
                        Products["7521"],
                        Products["7524"],
                        Products["7525"],
                        Products["7526"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="Deluxe",
                      Products = new List<Product>() { 
                        Products["7502"],
                        Products["7509"],
                        Products["7510"],
                        Products["7511"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="Ultimate",
                      Products = new List<Product>() { 
                        Products["7503"],
                        Products["7514"],
                        Products["7515"],
                        Products["7516"],
                      }
                   }
                 }
               },

               new ProductGroupAttrib() { Name = "Website",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                    new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="5-Page Website",
                      Products = new List<Product>() { 
                        Products["7521"],
                        Products["7524"],
                        Products["7525"],
                        Products["7526"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="10-Page Website",
                      Products = new List<Product>() { 
                        Products["7502"],
                        Products["7509"],
                        Products["7510"],
                        Products["7511"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="999-Page Website",
                      Products = new List<Product>() { 
                        Products["7503"],
                        Products["7514"],
                        Products["7515"],
                        Products["7516"],
                      }
                   }
                 }
               },

               
               new ProductGroupAttrib() { Name = "Diskspace",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                    new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="1 GB Disk Space",
                      Products = new List<Product>() { 
                        Products["7521"],
                        Products["7524"],
                        Products["7525"],
                        Products["7526"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="10 GB Disk Space",
                      Products = new List<Product>() { 
                        Products["7502"],
                        Products["7509"],
                        Products["7510"],
                        Products["7511"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="50 GB Disk Space",
                      Products = new List<Product>() { 
                        Products["7503"],
                        Products["7514"],
                        Products["7515"],
                        Products["7516"],
                      }
                   }
                 }
               },

               new ProductGroupAttrib() { Name = "Bandwidth",
                 AttribVals = new List<ProductGroupAtribVal>() { 
                    new ProductGroupAtribVal() { DisplayOrder =0, IsDefault=true, Name="150 GB Bandwidth",
                      Products = new List<Product>() { 
                        Products["7521"],
                        Products["7524"],
                        Products["7525"],
                        Products["7526"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =1, IsDefault=false, Name="500 GB Bandwidth",
                      Products = new List<Product>() { 
                        Products["7502"],
                        Products["7509"],
                        Products["7510"],
                        Products["7511"],
                      }
                   },
                   new ProductGroupAtribVal() { DisplayOrder =2, IsDefault=false, Name="1,000 GB Bandwidth",
                      Products = new List<Product>() { 
                        Products["7503"],
                        Products["7514"],
                        Products["7515"],
                        Products["7516"],
                      }
                   }
                 }
               }
             }
          };

      return offer;
    }
    #endregion
    
    #endregion
  }
}
