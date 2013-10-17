using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class RegistrationTypes
  {
    public const int BACKORDER = 0;
    public const int PRE_REGISTRATION = 1;
    public const int REGISTRATION = 2;
    public const int TRANSFER = 3;
    public const int BULK = 4;
    public const int BULK_TRANSFER = 5;
    public const int EMAIL_FORWARDING = 6;
    public const int AFTERMARKET = 100;
    public const int RENEW = 101;
    public const int BULK_RENEW = 102;
    public const int INTERNATIONAL = 103;
    public const int BULK_INTERNATIONAL = 104;
    public const int INTERNATIONAL_TRANSFER = 105;
    public const int BULK_INTERNATIONAL_TRANSFER = 106;
  }
}
