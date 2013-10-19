using System;
using System.Collections.Generic;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Providers.Basket.Interface;

namespace Atlantis.Framework.Providers.Basket
{
  public class ProviderBasketResponse : IBasketResponse
  {
    private readonly List<IBasketError> _errors;

    internal static IBasketResponse FromBasketResponseStatus(BasketResponseStatus responseStatus)
    {
      var result = new ProviderBasketResponse {Message = responseStatus.Status.ToString()};

      foreach (var error in responseStatus.Errors)
      {
        var basketError = new ProviderBasketError {Number = error.Number, Description = error.Description};
        result.AddError(basketError);
      }

      return result;
    }

    internal static IBasketResponse FromException(Exception ex)
    {
      var result = new ProviderBasketResponse {Message = ex.Message};
      var basketError = new ProviderBasketError {Number = "-1", Description = ex.Message + ex.StackTrace};
      result.AddError(basketError);
      return result;
    }

    private ProviderBasketResponse()
    {
      _errors = new List<IBasketError>();
      Message = string.Empty;
    }

    public string Message { get; private set; }
    public int ErrorCount
    {
      get { return _errors.Count; }
    }

    private void AddError(ProviderBasketError error)
    {
      _errors.Add(error);
    }

    public IEnumerable<IBasketError> Errors
    {
      get { return _errors; }
    }

    public bool TryGetError(string errorNumber, out IBasketError error)
    {
      error = null;

      if (string.IsNullOrEmpty(errorNumber))
      {
        return false;
      }

      error = _errors.Find(b => b.Number.Equals(errorNumber, StringComparison.OrdinalIgnoreCase));
      return (error != null);
    }
  }
}
