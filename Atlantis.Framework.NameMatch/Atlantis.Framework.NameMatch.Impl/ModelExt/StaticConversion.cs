using Atlantis.Framework.NameMatch.Impl.NameMatchService;

namespace Atlantis.Framework.NameMatch.Impl.ModelExt
{
  public static class StaticConversion
  {
    public static Interface.AvailableDomain ConvertAvailableDomain(this AvailableDomain ad)
    {
      var mapped = new Atlantis.Framework.NameMatch.Interface.AvailableDomain();
      mapped.DomainName = ad.DomainName;
      mapped.Extension = ad.Extension;

      mapped.NameWithoutExtension = ad.NameWithoutExtension;
      mapped.Keys = ad.Keys;

      Interface.DomainData[][] dd = new Interface.DomainData[ad.Data.Length][];
      for (int i = 0; i < ad.Data.Length; i++)
      {
        int innerLength = ad.Data[i].Length;
        dd[i] = new Interface.DomainData[innerLength];

        for (int j = 0; j < innerLength; j++)
        {
          dd[i][j] = ad.Data[i][j].Convert();
        }
      }
      mapped.Data = dd;

      mapped.AnchorWord = ad.AnchorWord;
      mapped.AvailCheckPerformed = ad.AvailCheckPerformed;
      mapped.SearchMethod = ad.SearchMethod;
      mapped.DomainAvailable = ad.DomainAvailable;
      mapped.FullDomainName = ad.FullDomainName;
      mapped.SLD = ad.SLD;
      mapped.TLD = ad.TLD;

      return mapped;
    }

  }
}
