using System;

namespace Atlantis.Framework.Tokens.Interface
{
  public abstract class TokenBase<T> : IToken where T : class
  {
    string _key;
    string _rawData;
    T _tokenDataObject;
    string _result = null;
    string _error = null;
    string _fullTokenString;

    public TokenBase(string key, string data, string fullTokenString)
    {
      _key = key;
      _rawData = data;
      _fullTokenString = fullTokenString;

      try
      {
        _tokenDataObject = DeserializeTokenData(data);
      }
      catch (Exception ex)
      {
        _tokenDataObject = null;
        _error = ex.Message;
      }
    }

    public string TokenKey
    {
      get { return _key; }
    }

    public T TokenData
    {
      get { return _tokenDataObject; }
    }

    public string FullTokenString
    {
      get { return _fullTokenString; }
    }

    public string RawTokenData
    {
      get { return _rawData; }
    }

    public string TokenResult
    {
      get { return _result; }
      set { _result = value; }
    }

    public string TokenError
    {
      get { return _error; }
      set { _error = value; }
    }

    protected abstract T DeserializeTokenData(string data);

  }
}
