using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Manager.Interface
{
  public class ManagerGetProductDetailResponseData : ResponseData
  {
    public static ManagerGetProductDetailResponseData FromDataTable(DataTable data)
    {
      return new ManagerGetProductDetailResponseData(data);
    }

    private ManagerGetProductDetailResponseData(DataTable data)
    {
      ProductCatalogDetails = data;
    }

    public DataTable ProductCatalogDetails { get; private set; }
  }
}