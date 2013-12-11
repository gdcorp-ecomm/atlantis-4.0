using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputPhoneNumberResponseData : ValidateInputResponseData
  {
    private const string _TYPENAME = "ValidateInputPhoneNumberResponseData";
    public ValidateInputPhoneNumberResponseData(ValidateInputResult result) : base(result) { }
    public ValidateInputPhoneNumberResponseData(ValidateInputResult result, RequestData requestData, Exception ex) : base(result, requestData, _TYPENAME, ex) { }

    public override string ToXML()
    {
      var element = new XElement(_TYPENAME);
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
