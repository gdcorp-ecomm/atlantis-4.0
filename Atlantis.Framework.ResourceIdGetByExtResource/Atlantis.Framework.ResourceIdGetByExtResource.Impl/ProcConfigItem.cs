
namespace Atlantis.Framework.ResourceIdGetByExtResource.Impl
{
  class ProcConfigItem
  {
    public int ProductTypeId { get; set; }
    public string BillingNamespace { get; set; }
    public string OrionNamespace { get; set; }
    public string StoredProcedure { get; set; }
    public string ConnectionString { get; set; }

    public ProcConfigItem(int productTypeId, string billingNamespace, string orionNamespace, string storedProcedure, string connectionString)
    {
      ProductTypeId = productTypeId;
      BillingNamespace = billingNamespace;
      OrionNamespace = orionNamespace;
      StoredProcedure = storedProcedure;
      ConnectionString = connectionString;
    }
  }
}

