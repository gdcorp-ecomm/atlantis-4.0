using System;
using System.Diagnostics;
using System.Threading;

namespace Atlantis.Framework.Interface
{
  public class ConfigElementStats
  {
    const int _TIMEARRAYSIZE = 100;
    int _succeeded = 0;
    int _failed = 0;
    DateTime _startTime = DateTime.Now;
    long[] _successMilliseconds = new long[_TIMEARRAYSIZE];
    long[] _failMilliseconds = new long[_TIMEARRAYSIZE];

    public void LogFailure(Stopwatch callTimer)
    {
      // Note: Interlocked.Increment will roll to minvalue automatically without an overflow error
      int failValue = Interlocked.Increment(ref _failed);

      if ((callTimer != null) && (callTimer.ElapsedMilliseconds > 0))
      {
        int timerIndex = Math.Abs(failValue % _TIMEARRAYSIZE);
        Interlocked.Exchange(ref _failMilliseconds[timerIndex], callTimer.ElapsedMilliseconds);
      }
    }

    public void LogSuccess(Stopwatch callTimer)
    {
      // Note: Interlocked.Increment will roll to minvalue automatically without an overflow error
      int successValue = Interlocked.Increment(ref _succeeded);

      if ((callTimer != null) && (callTimer.ElapsedMilliseconds > 0))
      {
        int timerIndex = Math.Abs(successValue % _TIMEARRAYSIZE);
        Interlocked.Exchange(ref _successMilliseconds[timerIndex], callTimer.ElapsedMilliseconds);
      }
    }

    public DateTime StartTime
    {
      get { return _startTime; }
    }

    public int Succeeded
    {
      get { return _succeeded; }
    }

    public int Failed
    {
      get { return _failed; }
    }

    private TimeSpan CalculateAvereage(long[] msArray)
    {
      TimeSpan result;
      double count = 0;
      long total = 0;

      foreach (long milliseconds in msArray)
      {
        if (milliseconds > 0)
        {
          count++;
          total = total + milliseconds;
        }
      }

      if (count == 0)
      {
        result = TimeSpan.Zero;
      }
      else
      {
        double average = total / count;
        result = TimeSpan.FromMilliseconds(average);
      }

      return result;
    }

    public TimeSpan CalculateAverageFailTime()
    {
      return CalculateAvereage(_failMilliseconds);
    }

    public TimeSpan CalculateAverageSuccessTime()
    {
      return CalculateAvereage(_successMilliseconds);
    }
  }
}
