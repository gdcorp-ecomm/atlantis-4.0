namespace Atlantis.Framework.DotTypeCache.Interface
{
// ReSharper disable InconsistentNaming
  public interface ITLDPreRegistration
// ReSharper restore InconsistentNaming
  {
    bool IsValidPreRegistrationPhase(string type, string subType, out ITLDPreRegistrationPhase preRegistrationPhase);
  }
}
