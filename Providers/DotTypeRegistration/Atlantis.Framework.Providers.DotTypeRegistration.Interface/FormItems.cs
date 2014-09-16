
namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public class FormItems : IFormItems
  {
    public string FormDescription { get; set; }
    public string FormLabel { get; set; }
    public string FormName { get; set; }
    public string FormType { get; set; }
    public string ValidationLevel { get; set; }
    public string FieldCollectionLabel { get; set; }
  }
}
