using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Interface
{
  public class LoginRequestData : RequestData
  {
    // Sample constructor
    // Do not use the base constructor any more, create a constructor like this that requires only what you need
    public LoginRequestData(object inputData)
    {
      // store input data into member properties
      // that will be accessible by the IRequest handler
    }
  }
}
