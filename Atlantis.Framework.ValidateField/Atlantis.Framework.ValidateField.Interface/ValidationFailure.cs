
namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidationFailure
  {
    private string DescriptionFormat { get; set; }

    public int FailureCode { get; private set; }

    public ValidationFailure(int failureCode, string description)
    {
      FailureCode = failureCode;
      DescriptionFormat = description;
    }

    public string GetFormattedDescription(string fieldName)
    {
      return string.Format(DescriptionFormat, fieldName);
    }
  }
}
