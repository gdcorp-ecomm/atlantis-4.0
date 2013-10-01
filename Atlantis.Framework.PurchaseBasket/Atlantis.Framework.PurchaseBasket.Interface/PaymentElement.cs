
using System;
using System.Collections.Generic;
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public abstract class PaymentElement : ElementBase
  {
    public PaymentElement Clone()
    {      
      PaymentElement newElement= this.MemberwiseClone() as PaymentElement;
      Type cloneType=this.GetType();
      PaymentElement currentItem = Activator.CreateInstance(this.GetType()) as PaymentElement;
      foreach (KeyValuePair<string, string> currentValue in newElement)
      {
        currentItem[currentValue.Key] = currentValue.Value;
      }
      return currentItem;
    }
  }
}
