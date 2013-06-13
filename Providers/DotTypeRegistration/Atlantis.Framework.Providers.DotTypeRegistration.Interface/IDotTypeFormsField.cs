﻿using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormsField
  {
    string FieldDescription { get; set; }
    string FieldType { get; set; }
    string FieldName { get; set; }
    string FieldLabel { get; set; }
    string DataSource { get; set; }
    string DataSourceMethod { get; set; }
    IList<IDotTypeFormsValidationRule> ValidationRuleCollection { get; set; }
    IList<IDotTypeFormsItem> ItemCollection { get; set; }
  }
}
