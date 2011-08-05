
namespace Atlantis.Framework.MessagingProcess.Interface
{
  public class AttributeValue
  {
    public AttributeValue()
    {
      WriteMethod = (int)AttributeValueWriteMethod.Standard;
    }

    public AttributeValue(string value)
    {
      Value = value;
      WriteMethod = (int)AttributeValueWriteMethod.Standard;
    }

    public AttributeValue(string value, int iWriteMethod)
    {
      Value = value;
      WriteMethod = iWriteMethod;
    }

    public int WriteMethod { get; set; }

    public string Value { get; set; }
  }
}
