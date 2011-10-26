using System.Collections.Generic;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class DepartmentIds : Dictionary<DepartmentType, int>
  {
    public DepartmentIds(int privateLabelId)
    {
      if (privateLabelId == 1)
      {
        LoadGoDaddyDepartmentIds();
      }
      else if (privateLabelId == 2)
      {
        LoadBlueRazorDepartmentIds();
      }
      else
      {
        LoadResellerDepartmentIds();
      }
    }

    private void LoadGoDaddyDepartmentIds()
    {
      this.Add(DepartmentType.CustomSiteDeptId, 25);
      this.Add(DepartmentType.TrainingDeptId, 1625);
      this.Add(DepartmentType.RecurringHostingDeptId, 1003);
    }

    private void LoadBlueRazorDepartmentIds()
    {
      this.Add(DepartmentType.CustomSiteDeptId, 200225);
      this.Add(DepartmentType.TrainingDeptId, 1625);
      this.Add(DepartmentType.RecurringHostingDeptId, 201003);
    }

    private void LoadResellerDepartmentIds()
    {
      this.Add(DepartmentType.CustomSiteDeptId, 225);
      this.Add(DepartmentType.TrainingDeptId, 1625);
      this.Add(DepartmentType.RecurringHostingDeptId, 1039);
    }
  }
}
