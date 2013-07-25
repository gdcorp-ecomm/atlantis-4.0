using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Geo.Interface
{
  /// <summary>
  /// IGeoCountry object
  /// </summary>
  public interface IGeoCountry
  {
    /// <summary>
    /// Id of country in database.  Is not consistent across environments.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Country code
    /// </summary>
    string Code { get; }
    
    /// <summary>
    /// Country name.  Will be localized if ILocalizationProvider is available
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Calling code
    /// </summary>
    string CallingCode { get; }

    /// <summary>
    /// Does this country have any states
    /// </summary>
    bool HasStates { get; }

    /// <summary>
    /// Collection of states for this country
    /// </summary>
    IEnumerable<IGeoState> States { get; }

    /// <summary>
    /// Tries to find the state by state code.
    /// </summary>
    /// <param name="stateCode">State code</param>
    /// <param name="state">IGeoState returned if found</param>
    /// <returns>true if found</returns>
    bool TryGetStateByCode(string stateCode, out IGeoState state);


  }
}
