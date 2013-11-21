using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class UsernameRule : RuleContainer
  {
    private string _username;
    private string _fieldName;
    private string _requestUrl = string.Empty;
    private string _pathway = string.Empty;
    private int _pageCount = 0;

    private bool _usernameExists;
    public bool UsernameAlreadyExists
    {
      get { return _usernameExists; }
    }

    /// <summary>
    /// Should ONLY be used when requestUrl, pathway, and pageCount are undefined.  Otherwise use constructor which accepts these parameters.
    /// </summary>
    public UsernameRule(string value, bool isNewShopper = false, string fieldName = "", bool isRequired = false, string culture = "")
      : base(value, culture)
    {
      var FieldNames = new FieldNames(Culture);
      DefaultFieldNameHelper.OverwriteTextIfEmpty(fieldName, FieldNames.Username, out fieldName);

      _username = value;
      _fieldName = fieldName;

      AddIsRequiredRule(value, fieldName, isRequired);
      base.RulesToValidate.Add(new InvalidCharactersRule(_fieldName, _username, Culture));
      base.RulesToValidate.Add(new MaxLengthRule(Culture, _fieldName, _username, LengthConstants.UsernameMaxLength));
      base.RulesToValidate.Add(new MinLengthRule(Culture, _fieldName, _username, LengthConstants.UsernameMinLength));

      if (isNewShopper)
      {
        BuildNewShopperRules();
      }
    }

    public UsernameRule(string value, string requestUrl, string pathway, int pageCount, bool isNewShopper = false, string fieldName = "", bool isRequired = false, string culture = "")
      : this(value, isNewShopper, fieldName, isRequired, culture)
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
        string formatString = FetchResource.GetString("cannotBeNumeric");
        base.RulesToValidate.Add(new BlankRule(false, string.Format(formatString, _fieldName), Culture));
      }
      else
      {
        var searchFields = new Dictionary<string, string>();
        var returnFields = new List<string>();

        searchFields["loginName"] = _username;
        returnFields.Add("loginName");

        SearchShoppersRequestData request = new SearchShoppersRequestData(string.Empty, "ShopperValidator::UsernameRule", searchFields, returnFields);
        SearchShoppersResponseData response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(request, EngineRequestValues.ShopperSearch);

        if (response.Count > 0)
        {
          _usernameExists = true;
          string formatString = FetchResource.GetString("alreadyExists");
          base.RulesToValidate.Add(new BlankRule(false, string.Format(formatString, _fieldName), Culture));
        }
      }
    }
  }
}
