using Atlantis.Framework.SearchShoppers.Interface;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class UsernameRule : SingleValueRuleContainer
  {
    private string _username;
    private string _fieldName;
    private string _requestUrl = string.Empty;
    private string _pathway = string.Empty;
    private int _pageCount = 0;

    /// <summary>
    /// Should ONLY be used when requestUrl, pathway, and pageCount are undefined.  Otherwise use constructor which accepts these parameters.
    /// </summary>
    public UsernameRule(string value, bool isNewShopper = false, string fieldName = FieldNames.Username, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      _username = value;
      _fieldName = fieldName;

      //base.RulesToValidate.Add(new RequiredRule(_fieldName, _username));
      base.RulesToValidate.Add(new InvalidCharactersRule(_fieldName, _username));
      base.RulesToValidate.Add(new MaxLengthRule(_fieldName, _username, LengthConstants.UsernameMaxLength));
      base.RulesToValidate.Add(new MinLengthRule(_fieldName, _username, LengthConstants.UsernameMinLength));

      if (isNewShopper)
      {
        BuildNewShopperRules();
      }
    }
    
    public UsernameRule(string value, string requestUrl, string pathway, int pageCount, bool isNewShopper = false, string fieldName = FieldNames.Username, bool isRequired = false)
      : this(value, isNewShopper, fieldName, isRequired)
    {
      _requestUrl = requestUrl;
      _pageCount = pageCount;
      _pathway = pathway;
    }
      
    private void BuildNewShopperRules()
    {
      //Login cannot be all numbers
      if (RegexConstants.NumericOnly.IsMatch(_username))
      {
        base.RulesToValidate.Add(new BlankRule(false, string.Concat(_fieldName, " cannot be only numbers")));
      }
      else
      {
        SearchShoppersRequestData loginRequest = new SearchShoppersRequestData(_username, _requestUrl, string.Empty, _pathway, _pageCount, "ShopperValidator::UsernameRule");
        loginRequest.AddSearchField("loginName", _username);
        loginRequest.AddReturnField("loginName");

        SearchShoppersResponseData loginResponse =  (SearchShoppersResponseData)Engine.Engine.ProcessRequest(loginRequest, EngineRequestValues.SearchShoppers);
        if (loginResponse.ShopperCount > 0)
        {
          base.RulesToValidate.Add(new BlankRule(false, string.Concat(_fieldName, " already exists")));
        }
      }
    }
  }
}
