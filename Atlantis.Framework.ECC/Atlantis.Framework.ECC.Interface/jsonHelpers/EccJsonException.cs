using System;

namespace Atlantis.Framework.Ecc.Interface
{
  public class EccJsonException : Exception
  {
    public  EccJsonException(string message) : base(message)
    {
      StatusCode = -1;
    }

    public EccJsonException(int responseCode, string message)
      : base(message)
    {
      StatusCode = responseCode;
    }


    public int StatusCode { get; private set; }
    
  }
}