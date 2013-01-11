namespace Atlantis.Framework.RegDotTypeRegistry.Interface
{
  public class RegistryApi 
  {
    public string Id { get; private set; }
    public string Description { get; private set; }

    internal RegistryApi(string registryId, string registryDescription)
    {
      Id = registryId;
      Description = registryDescription;
    }
  }
}
