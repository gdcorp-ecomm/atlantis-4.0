namespace Atlantis.Framework.Ecc.Interface.Enums
{
  public enum EccEmailAddressStatus
  {
    PendingSetup = 0,
    Active = 10,
    PendingDelete = 20,
    PendingBlock = 30,
    PendingWebSetup = 40,
    PendingError = 50,
    PendingUnblock = 60,
    CurrentlyBlocked = 70,
    PendingMigration = 71,
    MigrationInProgress = 72,
    PendingMxValidation = 80,
    MxValidationFailure = 89,
    Deleted = 90
  }
}
