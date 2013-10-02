using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
// ReSharper disable InconsistentNaming
  public class TldTuiFormGroup : ITLDTuiFormGroup
// ReSharper restore InconsistentNaming
  {
    public string Type { get; private set; }
    public string Value { get; private set; }

    private readonly HashSet<TldTuiFormGroupLaunchPhase> _formgroupLaunchPhases;
    public IEnumerable<ITLDTuiFormGroupLaunchPhase> FormGrouplaunchPhases
    {
      get { return _formgroupLaunchPhases; }
    }

    private static readonly TldTuiFormGroup _nullTuiFormGroup;

    static TldTuiFormGroup()
    {
      _nullTuiFormGroup = FromNothing();
    }

    public static TldTuiFormGroup NULLTUIGROUP
    {
      get { return _nullTuiFormGroup; }
    }

    public static TldTuiFormGroup FromFormGroupElement(XElement formGroupElement)
    {
      return new TldTuiFormGroup(formGroupElement);
    }

    public static TldTuiFormGroup FromNothing()
    {
      return null;
    }

    private TldTuiFormGroup(XElement formGroupElement)
    {
      var formType = formGroupElement.Attribute("type");
      if (formType != null)
      {
        Type = formType.Value;
      }

      var formValue = formGroupElement.Attribute("value");
      if (formValue != null)
      {
        Value = formValue.Value;
      }

      //populate form group launch phases
      _formgroupLaunchPhases = new HashSet<TldTuiFormGroupLaunchPhase>();
      foreach (var launchPhase in formGroupElement.Descendants("formgrouplaunchphase"))
      {
        var formGrouplaunchPhase = new TldTuiFormGroupLaunchPhase();

        var codeAttr = launchPhase.Attribute("launchphasecode");
        var periodTypeAttr = launchPhase.Attribute("launchphaseperiodtype");

        if (codeAttr != null)
        {
          formGrouplaunchPhase.Code = codeAttr.Value;
        }

        if (periodTypeAttr != null)
        {
          formGrouplaunchPhase.PeriodType = periodTypeAttr.Value;
        }

        _formgroupLaunchPhases.Add(formGrouplaunchPhase);
      }
    }
  }
}
