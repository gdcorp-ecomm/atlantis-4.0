namespace Atlantis.Framework.Manager.Interface.ManagerUser
{
  public class ManagerUserData
  {
    private readonly string _userId;
    public string UserId
    {
      get { return _userId; }
    }

    private readonly string _fullName;
    public string FullName
    {
      get { return _fullName; }
    }

    private readonly string _loginName;
    public string LoginName
    {
      get { return _loginName; }
    }

    private readonly string _mstk;
    public string Mstk
    {
      get { return _mstk; }
    }

    public ManagerUserData(string userId, string firstName, string lastName, string loginName, string mstk)
    {
      _userId = userId;
      _fullName = string.Concat(firstName, " ", lastName);
      _loginName = loginName;
      _mstk = mstk;
    }
  }
}
