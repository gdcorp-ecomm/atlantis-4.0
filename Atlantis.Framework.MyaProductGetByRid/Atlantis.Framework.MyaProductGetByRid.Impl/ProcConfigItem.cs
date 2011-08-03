
namespace Atlantis.Framework.MyaProductGetByRid.Impl
{
  class ProcConfigItem
  {
    public int ProductTypeId { get; set; }
    public string BillingNamespace { get; set; }
    public string StoredProcedure { get; set; }
    public string ConnectionString { get; set; }

    public ProcConfigItem(int productTypeId, string billingNamespace, string storedProcedure, string connectionString)
    {
      ProductTypeId = productTypeId;
      BillingNamespace = billingNamespace;
      StoredProcedure = storedProcedure;
      ConnectionString = connectionString;
    }
  }
}
