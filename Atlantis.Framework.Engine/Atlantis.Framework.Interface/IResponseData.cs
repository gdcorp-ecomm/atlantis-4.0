
namespace Atlantis.Framework.Interface
{
  public interface IResponseData
  {
    string ToXML();
    AtlantisException GetException();
  }
}
