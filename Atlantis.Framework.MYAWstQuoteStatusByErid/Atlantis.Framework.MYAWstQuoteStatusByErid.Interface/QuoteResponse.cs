
namespace Atlantis.Framework.MYAWstQuoteStatusByErid.Interface
{
  public class QuoteResponse
  {
    private string _error;
    private string _id;
    private bool _isSuccess;
    private string _statusId;
    private string _statusDescription;

    public string Id
    {
      get { return _id; }
      set { _id = value; }
    }

    public string StatusId
    {
      get { return _statusId; }
      set { _statusId = value; }
    }

    public string StatusDescription
    {
      get { return _statusDescription; }
      set { _statusDescription = value; }
    }

    public string Error
    {
      get { return _error; }
      set { _error = value; }
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
      set { _isSuccess = value; }
    }
  }
}
