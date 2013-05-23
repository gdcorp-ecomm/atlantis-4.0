﻿using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsField : IDotTypeFormsField
  {
    public string FieldDescription { get; set; }
    public string FieldType { get; set; }
    public string FieldName { get; set; }
    public string FieldFactName { get; set; }
    public string FieldLabel { get; set; }
    public IList<IDotTypeFormsValidationRule> ValidationRuleCollection { get; set; }
    public IList<IDotTypeFormsItem> ItemCollection { get; set; }
  }
}