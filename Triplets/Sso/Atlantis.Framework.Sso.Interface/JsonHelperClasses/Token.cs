
using Atlantis.Framework.Interface;
using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class Token : AuthApiResponseBase
  {
    #region Properties
    private readonly Lazy<JavaScriptSerializer> _serializer;
    private readonly Lazy<bool> _isSignatureValid;
    private Key _key;

    public string data { get; set; }
    public int PrivateLabelId { get; set; }

    public string Signature { get { return RawTokenData.Signature; } }

    private Payload _payload;
    public Payload Payload
    {
      get
      {
        if (_payload == null)
        {
          _payload = new Payload();

          if (_isSignatureValid.Value && RawTokenData.Payload.Length > 0)
          {
            string decryptedData = DecryptionHelper.Base64UrlDecodeToString(RawTokenData.Payload);
            _payload = _serializer.Value.Deserialize<Payload>(decryptedData);
          }
        }

        return _payload;
      }
    }

    private Header _header;
    public Header Header
    {
      get
      {
        if (_header == null)
        {
          _header = new Header();

          if (RawTokenData.Header.Length > 0)
          {
            string decryptedData = DecryptionHelper.Base64UrlDecodeToString(RawTokenData.Header);
            _header = _serializer.Value.Deserialize<Header>(decryptedData);
          }
        }

        return _header;
      }
    }

    private readonly Lazy<RawTokenData> _rawTokenData;
    public RawTokenData RawTokenData
    {
      get { return _rawTokenData.Value; }
    }

    public bool IsSignatureValid
    {
      get { return _isSignatureValid.Value; }
    }

    private DateTime? _issuedAtTime;
    public DateTime IssuedAt
    {
      get
      {
        if (!_issuedAtTime.HasValue)
        {
          _issuedAtTime = EpochTimeHelper.FromUnixEpoch(Payload.iat);
        }

        return _issuedAtTime.Value;
      }
    }

    private DateTime? _expiresAtTime;
    public DateTime ExpiresAt
    {
      get
      {
        if (!_expiresAtTime.HasValue)
        {
          _expiresAtTime = EpochTimeHelper.FromUnixEpoch(Payload.exp);
        }

        return _expiresAtTime.Value;
      }
    }

    #endregion

    public Token()
    {
      _rawTokenData = new Lazy<RawTokenData>(() => GetRawTokenData());
      _serializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
      _isSignatureValid = new Lazy<bool>(() => ValidateSignature());

      data = string.Empty;
    }

    /// <summary>
    /// Validates the token signature using the public key from the auth api webservice. 
    /// </summary>
    /// <returns></returns>

    private bool ValidateSignature()
    {
      var isSignatureValid = false;

      try
      {
        bool tokenIsMissingRawData = string.IsNullOrEmpty(RawTokenData.Header) || string.IsNullOrEmpty(RawTokenData.Payload) || string.IsNullOrEmpty(RawTokenData.Signature);

        if (!string.IsNullOrEmpty(Header.kid) && !tokenIsMissingRawData)
        {
          if (_key == null)
          {
            _key = GetKeyData();
          }
            var publicKey = new RSAParameters { 
                Exponent = DecryptionHelper.Base64UrlDecodeToBytes(_key.data.e),
                Modulus = DecryptionHelper.Base64UrlDecodeToBytes(_key.data.n)
              };

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(publicKey);

            var verifyData = string.Concat(RawTokenData.Header, ".", RawTokenData.Payload);
            var bytesToVerify = Encoding.UTF8.GetBytes(verifyData);
            var signedBytes = DecryptionHelper.Base64UrlDecodeToBytes(RawTokenData.Signature);

            HashAlgorithm hash = HashAlgorithm.Create("SHA256");
            isSignatureValid = rsa.VerifyData(bytesToVerify, hash, signedBytes);

            if (!isSignatureValid)
            {
              throw new SecurityException("Signature is invalid for token.  Potential abuse detected");
            }
          

        }
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("Token::ValidateSignature", 1, ex.Message, "RawTokenData:" + RawTokenData));
      }

      return isSignatureValid;
    }

    private RawTokenData GetRawTokenData()
    {
      var rawTokenData = new RawTokenData();

      if (data.Length > 0)
      {
        string[] rawPieces = data.Split('.');
        if (rawPieces.Length >= 3)
        {
          rawTokenData.Header = rawPieces[0];
          rawTokenData.Payload = rawPieces[1];
          rawTokenData.Signature = rawPieces[2];
        }
      }

      return rawTokenData;
    }

    private Key GetKeyData()
    {
      int engineRequestType = SsoEngineRequests.SsoGetKeyRequest;

      if (PrivateLabelId == PrivateLabelIds.DomainsByProxy)
      {
        engineRequestType = SsoEngineRequests.SsoDBPGetKeyRequest;
      }
      else if (PrivateLabelId != PrivateLabelIds.GoDaddy)
      {
        engineRequestType = SsoEngineRequests.SsoPLGetKeyRequest;
      }

      var keyRequest = new SsoGetKeyRequestData(this);
      var keyResponse = (SsoGetKeyResponseData) DataCache.DataCache.GetProcessRequest(keyRequest, engineRequestType);

      return keyResponse.Key;
    }
  }
}
