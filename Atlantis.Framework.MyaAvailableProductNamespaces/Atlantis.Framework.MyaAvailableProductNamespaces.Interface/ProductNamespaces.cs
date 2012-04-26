
using System.Runtime.Serialization;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Interface
{
  [DataContract]
  public class ProductNamespace
  {
    public ProductNamespace(string name, string descrption, string example, string note, string sortOrder, string productGroupId)
    {
      Namespace = name;
      Description = descrption;
      Example = example;
      Note = note;
      SortOrder = sortOrder;
      ProductGroupId = productGroupId;
    }

    [DataMember(Name = "ns")]
    public string Namespace { get; private set; }
    [DataMember(Name = "desc")]
    public string Description { get; private set; }
    [DataMember(Name = "ex")]
    public string Example { get; private set; }
    [DataMember(Name = "note")]
    public string Note { get; private set; }
    [DataMember(Name = "sort")]
    public string SortOrder { get; private set; }
    [DataMember(Name = "pgrpid")]
    public string ProductGroupId { get; private set; }
  }
}
