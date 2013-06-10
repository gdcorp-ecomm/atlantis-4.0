using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ShopperValidator.Interface
{
  class ModelConstants
  {
    public static string MODEL_ID_SHOPPERVALID = "Shopper";

    public static string FACT_USERNAME = "txtUsername";
    public static string FACT_PASSWORD = "txtCreatePassword";
    public static string FACT_PASSWORD2 = "txtCreatePassword2";
    public static string FACT_EMAIL = "txtEmail";

    public static string FACT_USERNAME_MAX_LENGTH = "usernameMaxLength";
    public static string FACT_PASS_MAX_LENGTH = "passwordMaxLength";
    public static string FACT_PASS_MIN_LENGTH = "passwordMinLength";
    public static string FACT_PASS_REGEX = "passwordRegex";
    public static string FACT_EMAIL_REGEX = "emailRegex";
    public static string FACT_EMAIL_MAX_LENGTH = "emailMaxLength";
    public static string FACT_NUMERIC_ONLY_REGEX = "numericOnlyRegex";
    public static string FACT_INVALID_CHARACTERS_REGEX = "invalidCharsRegex";
  }
}
