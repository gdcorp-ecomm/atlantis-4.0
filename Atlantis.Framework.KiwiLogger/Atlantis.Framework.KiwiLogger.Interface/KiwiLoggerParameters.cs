
namespace Atlantis.Framework.KiwiLogger.Interface
{
  public class KiwiLoggerParameters
  {
    private string _fieldName;
    private string _fieldValue;

    public string FieldName
    {
      get { return _fieldName; }
    }

    public string FieldValue
    {
      get { return _fieldValue; }
    }

    public override string ToString()
    {
      return FieldName + "=" + FieldValue;

    }
    public KiwiLoggerParameters(string fieldName, string fieldValue)
    {
      _fieldName = fieldName;
      _fieldValue = fieldValue;
    }
  }
}
