namespace Atlantis.Framework.MailApi.Interface
{
  public class GetFolderListResponseData : MailApiResponseBase
  {
    // Sample static method for creating the response (ref Clean Code)
    public static GetFolderListResponseData FromData(object data)
    {
      return new GetFolderListResponseData(data);
    }

    // Sample private constructor
    private GetFolderListResponseData(object data)
    {
      // Sample: Create member data as necessary
    }

  }
}
