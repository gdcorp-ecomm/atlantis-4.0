using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourceIdGetByExtResource.Interface
{
  public class ResourceIdGetByExtResourceResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public int ResourceId { get; set; }
    public int ProductTypeId { get; set; }

    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public ResourceIdGetByExtResourceResponseData(int resourceId, int productTypeId)
    {
      ResourceId = resourceId;
      ProductTypeId = productTypeId;
    }

    public ResourceIdGetByExtResourceResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
      ResourceId = 0;
      ProductTypeId = 0;
    }
    
    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion

  }
}
