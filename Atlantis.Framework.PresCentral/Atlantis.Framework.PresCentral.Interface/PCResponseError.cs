using System.Xml.Linq;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCResponseError
  {
    public int ErrorNumber { get; private set; }
    public string Message { get; private set; }

    internal PCResponseError(XElement errorElement)
    {
      ErrorNumber = -1;
      Message = "unknown error";

      if (errorElement != null)
      {
        XAttribute numAtt = errorElement.Attribute("number");
        int number;
        if ((numAtt != null) && (int.TryParse(numAtt.Value, out number)))
        {
          ErrorNumber = number;
        }

        if (!string.IsNullOrEmpty(errorElement.Value))
        {
          Message = errorElement.Value;
        }
      }
    }
  }
}
