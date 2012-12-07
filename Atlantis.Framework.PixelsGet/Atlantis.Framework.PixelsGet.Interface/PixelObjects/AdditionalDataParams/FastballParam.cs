
namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.AdditionalDataParams
{
  public class FastballParam : AdditionalDataParam
  {
    private const string _ELEMENT_NAME = "fastball";
    private const string _ATTRIBUTE_NAME = "cicode";

    public override string ElementName
    {
      get { return _ELEMENT_NAME; }
    }

    public override string AttributeName
    {
      get
      {
        return _ATTRIBUTE_NAME;
      }
    }
  }
}
