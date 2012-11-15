﻿namespace Atlantis.Framework.OAuth.Interface.Errors
{
  public struct AuthTokenResponseErrorCodes
  {
    public const string InvalidRequest = "invalid_request";
    public const string Unauthorized = "unauthorized_client";
    public const string AccessDenied = "access_denied";
    public const string InvalidResponseType = "unsupported_response_type";
    public const string InvalidScope = "invalid_scope";
    public const string InternalServerError = "server_error";
    public const string TemporarilyUnavailable = "temporarily_unavailable";
  }
}
