using System.Globalization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.UserAgentEx.Interface
{
  public class UserAgentExRequestData : RequestData
  {
    public int ExpressionType { get; private set; }

    public UserAgentExRequestData(int expressionType)
    {
      ExpressionType = expressionType;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(ExpressionType.ToString(CultureInfo.InvariantCulture));
    }
  }
}
