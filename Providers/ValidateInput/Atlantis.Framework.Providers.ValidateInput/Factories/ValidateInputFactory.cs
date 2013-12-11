using Atlantis.Framework.Providers.ValidateInput.Handlers;
using Atlantis.Framework.Providers.ValidateInput.Interface;

namespace Atlantis.Framework.Providers.ValidateInput.Factories
{
  public static class ValidateInputFactory
  {
    public static IValidateInputHandler GetHandler(ValidateInputTypes inputType)
    {
      IValidateInputHandler handler = null;

      switch (inputType)
      {
        case ValidateInputTypes.Password:
          handler = new ValidateInputPasswordHandler();
          break;
        case ValidateInputTypes.PhoneNumber:
          handler = new ValidateInputPhoneNumberHandler();
          break;
      }

      return handler;
    }
  }
}