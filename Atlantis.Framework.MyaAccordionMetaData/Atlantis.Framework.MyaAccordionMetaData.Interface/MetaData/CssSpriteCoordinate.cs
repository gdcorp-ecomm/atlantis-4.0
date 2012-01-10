
namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class CssSpriteCoordinate
  {
    #region ReadOnly Properties
    private readonly string _x;
    public string X
    {
      get { return _x; }
    }
    private readonly string _y;
    public string Y
    {
      get { return _y; }
    }
    private string _width;
    public string Width
    {
      get { return _width; }
    }
    private string _height;
    public string Height
    {
      get { return _height; }
    }
    #endregion

    public CssSpriteCoordinate(string x, string y, string width, string height)
    {
      _x = x;
      _y = y;
      _width = width;
      _height = height;
    }
  }
}
