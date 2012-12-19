namespace Atlantis.Framework.PromoPackageInfo.Interface
{
  public class PackageItem
  {
    public PackageItem(string packageDescription,
                       int injectedAmount,
                       bool includeFees,
                       int prs_packageGroupID,
                       string packageGroupDescription,
                       int groupQuantityAllowed,
                       int groupDisplayOrder,
                       int pf_id,
                       int quantity,
                       decimal duration)
    {
      PackageDescription = packageDescription;
      InjectedAmount = injectedAmount;
      IncludeFees = includeFees;
      PackageGroupID = prs_packageGroupID;
      PackageGroupDescription = packageGroupDescription;
      GroupQuantityAllowed = groupQuantityAllowed;
      GroupDisplayOrder = groupDisplayOrder;
      PfId = pf_id;
      Quantity = quantity;
      Duration = duration;
    }

    public string PackageDescription { get; private set; }
    public int InjectedAmount { get; private set; }
    public bool IncludeFees { get; private set; }
    public int PackageGroupID { get; private set; }
    public string PackageGroupDescription { get; private set; }
    public int GroupQuantityAllowed { get; private set; }
    public int GroupDisplayOrder { get; private set; }
    public int PfId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Duration { get; private set; }
  }
}