namespace Atlantis.Framework.Providers.MailApi.DTOs
{

  // CompositeLoginResponse is just a temp name for now
  // holds login data, folders list, message list


  public class CompositeLoginResponse
  {
    private Framework.MailApi.Interface.MailApiResponseState mailApiResponseState;
    private Framework.MailApi.Interface.GetMessageListData getMessageListData;
    private Framework.MailApi.Interface.LoginData loginData;
    private Framework.MailApi.Interface.MailFolderArray mailFolderArray;

    public Framework.MailApi.Interface.MailApiResponseState MailApiResponseState
    {
      get { return mailApiResponseState; }
      set { mailApiResponseState = value; }
    }

    public Framework.MailApi.Interface.LoginData LoginData
    {
      get { return loginData; }
      set { loginData = value; }
    }

    public Framework.MailApi.Interface.MailFolderArray MailFolderArray
    {
      get { return mailFolderArray; }
      set { mailFolderArray = value; }
    }

    public Framework.MailApi.Interface.GetMessageListData GetMessageListData
    {
      get { return getMessageListData; }
      set { getMessageListData = value; }
    }

    public CompositeLoginResponse(Framework.MailApi.Interface.MailApiResponseState mailApiResponseState, Framework.MailApi.Interface.LoginData loginData, Framework.MailApi.Interface.MailFolderArray mailFolderArray, Framework.MailApi.Interface.GetMessageListData getMessageListData)
    {
      // TODO: Complete member initialization
      this.mailApiResponseState = mailApiResponseState;
      this.loginData = loginData;
      this.mailFolderArray = mailFolderArray;
      this.getMessageListData = getMessageListData;
    }
  }
}
