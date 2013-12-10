using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Shopper.Interface
{
  public interface IShopperDataProvider
  {
    /// <summary>
    /// Registers fields that will be needed when running GetShopper request when TryGetField is called
    /// </summary>
    /// <param name="fields">one or more ShopperDataFields</param>
    void RegisterNeededFields(params string[] fields);

    /// <summary>
    /// Registers fields that will be needed when running GetShopper request when TryGetField is called
    /// </summary>
    /// <param name="fields">IEnumerable collection of ShopperDataFields</param>
    void RegisterNeededFields(IEnumerable<string> fields);

    /// <summary>
    /// Attempts to retrieve a shopper field value from the provider. If the requested field has not been
    /// Queried from FortKnox, GetShopper will be called to get the field.  Please use the RegisterNeededFields
    /// in your site's BeginRequest to minimize the number of GetShopper calls during your request.
    /// </summary>
    /// <typeparam name="T">Type to attempt to cast the field value to.  If it cannot be cast, then the result will be false.</typeparam>
    /// <param name="fieldName">ShopperDataField to get.</param>
    /// <param name="fieldValue">Cast output of the field value</param>
    /// <returns>true if the field was retrieved and successfully cast.</returns>
    bool TryGetField<T>(string fieldName, out T fieldValue);

    /// <summary>
    /// Verifies that the existing IShopperContext shopperid is a valid shopper.
    /// </summary>
    /// <returns>true if the existing shoppercontext has a valid shopper for the current private label id</returns>
    bool IsShopperValid();

    /// <summary>
    /// Creates a new shopper and sets the shopper into the shopper context (Public)
    /// </summary>
    /// <returns>true if the shopper was successfully created and set into context.</returns>
    bool TryCreateNewShopper();

    /// <summary>
    /// Updates the current IShopperContext shopper with the given field values.
    /// </summary>
    /// <param name="updates">IDictionary where the key is the fieldname and the value is the value to update it to.</param>
    /// <returns>true if the shopper was successfully updated.</returns>
    bool TryUpdateShopper(IDictionary<string, string> updates);

    /// <summary>
    /// Updates the current IShopperContext shopper with the given field values.
    /// </summary>
    /// <param name="updates">IDictionary where the key is the fieldname and the value is the value to update it to.</param>
    /// <returns>ShopperUpdateResultType enum.</returns>
    ShopperUpdateResultType UpdateShopperInfo(IDictionary<string, string> updates);
  }
}
