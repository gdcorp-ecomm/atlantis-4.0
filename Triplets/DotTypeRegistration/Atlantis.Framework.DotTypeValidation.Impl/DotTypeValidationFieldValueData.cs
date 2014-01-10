using Atlantis.Framework.DotTypeValidation.Interface;

namespace Atlantis.Framework.DotTypeValidation.Impl
{
  public class DotTypeValidationFieldValueData : IDotTypeValidationFieldValueData
  {
    public bool NoValidate { get; set; }
    public string Value { get; set; }

    private DotTypeValidationFieldValueData(string value, bool novalidate = false)
    {
      NoValidate = novalidate;
      Value = value;
    }

    public static IDotTypeValidationFieldValueData Create(string value, bool novalidate = false)
    {
      return new DotTypeValidationFieldValueData(value, novalidate);
    }
  }
}
