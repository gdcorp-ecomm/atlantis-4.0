using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Basket.Interface
{
  public class BasketItemCountResponseData : ResponseData
  {
    public static BasketItemCountResponseData Empty { get; private set; }

    static BasketItemCountResponseData()
    {
      Empty = new BasketItemCountResponseData(0);
    }

    public static BasketItemCountResponseData FromPipeDelimitedResponseString(string cartTotalsResponseString)
    {
      if (string.IsNullOrEmpty(cartTotalsResponseString))
      {
        return Empty;
      }

      int pipePosition = cartTotalsResponseString.IndexOf('|');
      if (pipePosition > -1)
      {
        cartTotalsResponseString = cartTotalsResponseString.Substring(0, pipePosition);
      }

      if (string.IsNullOrEmpty(cartTotalsResponseString))
      {
        return Empty;
      }

      int total;
      if (!int.TryParse(cartTotalsResponseString, out total))
      {
        return Empty;
      }

      if (total == 0)
      {
        return Empty;
      }

      return new BasketItemCountResponseData(total);
    }

    public int TotalItems { get; private set; }

    private BasketItemCountResponseData(int totalItems)
    {
      TotalItems = totalItems;
    }
  }
}
