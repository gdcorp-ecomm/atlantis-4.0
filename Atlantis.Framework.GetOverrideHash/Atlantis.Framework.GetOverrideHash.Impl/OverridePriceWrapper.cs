using System;
using System.Runtime.InteropServices;

using gdOverrideLib;

namespace Atlantis.Framework.GetOverrideHash.Impl
{
  class OverridePriceWrapper : IDisposable
  {
    public OverridePriceWrapper()
    {
      Price = new PriceClass();
    }

    public OverridePriceWrapper(string type)
    {
      PriceType = new PriceType();
    }

    public PriceClass Price { get; private set; }
    public PriceType PriceType { get; private set; }

    #region IDisposable Members

    void IDisposable.Dispose()
    {
      if (Price != null)
      {
        Marshal.ReleaseComObject(Price);
        Price = null;
      }
      if (PriceType != null)
      {
        Marshal.ReleaseComObject(PriceType);
        PriceType = null;
      }
      GC.SuppressFinalize(this);
    }

    #endregion

    ~OverridePriceWrapper()
    {
      if (Price != null)
      {
        Marshal.ReleaseComObject(Price);
        Price = null;
      }
      if (PriceType != null)
      {
        Marshal.ReleaseComObject(PriceType);
        PriceType = null;
      }
    }
  }
}
