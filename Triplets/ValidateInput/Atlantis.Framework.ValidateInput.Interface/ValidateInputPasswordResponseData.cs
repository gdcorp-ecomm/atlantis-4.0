using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidateInputPasswordResponseData : ValidateInputResponseData
  {
    private const string _TYPENAME = "ValidateInputPasswordResponseData";
    public ValidateInputPasswordResponseData(ValidateInputResult result) : base(result) { }
    public ValidateInputPasswordResponseData(ValidateInputResult result, RequestData requestData, Exception ex) : base(result, requestData, _TYPENAME, ex) { }

    public override string ToXML()
    {
      var element = new XElement(_TYPENAME);
      return element.ToString(SaveOptions.DisableFormatting);
    }
  }
}
