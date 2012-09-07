using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;

namespace Atlantis.Framework.ShopperValidator.Interface
{
 public class ShopperValidatorRequestData: RequestData
  {
   public ShopperToValidate ShopperToValidate;
   public bool IsNewShopper;

   public ShopperValidatorRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, ShopperToValidate shopperToValidate, bool isNewShopper)
     : base(shopperId, sourceURL, orderId, pathway, pageCount)
   {
     ShopperToValidate = shopperToValidate;
     IsNewShopper = isNewShopper;
   }

   public override string GetCacheMD5()
   {
     throw new NotImplementedException();
   }
  }
}
