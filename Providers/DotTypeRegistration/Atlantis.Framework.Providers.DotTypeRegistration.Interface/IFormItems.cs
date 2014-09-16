
namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IFormItems
  {
    string FormDescription { get; set; }
    string FormLabel { get; set; }
    string FormName { get; set; }
    string FormType { get; set; }
    string ValidationLevel { get; set; }
    string FieldCollectionLabel { get; set; }
  }
}
