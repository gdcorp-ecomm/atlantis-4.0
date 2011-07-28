using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ChangeAccount.Interface
{
  public class ChangeAccountResponseObject
  {

    public string ResourceID { get; set; }
    public int RenewalPFID { get; set; }

    public bool IsSuccess { get; set; }
    public string XML { get; set; }
    public string ToXML()
    {
      return XML;
    }

    public AtlantisException AtlException { get; set; }
    public AtlantisException GetException()
    {
      return AtlException;
    }

    public int BasketResultCode { get; set; }
  }
}
