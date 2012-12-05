using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardIsValid.Interface
{
  public class EcommGiftCardIsValidRequestData : RequestData
  {

    private string _acctNumber = string.Empty;

    public EcommGiftCardIsValidRequestData(string shopperId,
                  string sourceUrl,
                  string orderId,
                  string pathway,
                  int pageCount,
                  string acctNumber)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _acctNumber = acctNumber;
    }

    public string AccountNumber
    {
      get { return _acctNumber; }
      set { _acctNumber = value; }
    }

    public override string ToXML()
    {
      return string.Empty;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

  }
}
