
namespace Atlantis.Framework.NameMatch.Impl.NameMatchService
{
  public partial class DomainData
  {
    public Interface.DomainData Convert()
    {
      var mapped = new Atlantis.Framework.NameMatch.Interface.DomainData();
      mapped.Data = this.Data;
      mapped.Name = this.Name;

      return mapped;
    }
  }
}
