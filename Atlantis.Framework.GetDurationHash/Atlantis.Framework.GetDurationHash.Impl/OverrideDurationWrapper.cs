using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.GetDurationHash.Impl
{
  class OverrideDurationWrapper : IDisposable
  {
    private gdOverrideLib.DurationClass m_duration;

    public OverrideDurationWrapper()
    {
      m_duration = new gdOverrideLib.DurationClass();
    }

    public gdOverrideLib.DurationClass Duration
    {
      get { return m_duration; }
    }

    #region IDisposable Members

    // **************************************************************** //

    void IDisposable.Dispose()
    {
      if (m_duration != null)
      {
        Marshal.ReleaseComObject(m_duration);
        m_duration = null;
      }
      GC.SuppressFinalize(this);
    }

    // **************************************************************** //

    #endregion

    ~OverrideDurationWrapper()
    {
      if (m_duration != null)
      {
        Marshal.ReleaseComObject(m_duration);
        m_duration = null;
      }
    }

  }
}
