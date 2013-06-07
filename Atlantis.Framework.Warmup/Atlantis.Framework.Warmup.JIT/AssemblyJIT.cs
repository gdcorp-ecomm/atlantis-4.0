namespace Atlantis.Framework.Warmup.JIT
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Threading;
  using Atlantis.Framework.Warmup.JIT.Interfaces;

  /// <summary>
  /// Class that will JIT assemblies.
  /// This class is not thread-safe.  For its intended usage, it need not be.
  /// In addition, the life of the thread that is created for JIT'ing is not
  /// managed/maintained or terminated when the class is destructed... this 
  /// is by design.  The class wasn't designed to block the termination of a 
  /// site if the compile thread was busy, unresponsive, etc.
  /// </summary>
  public class AssemblyJIT : IAssemblyJIT
  {
    public AssemblyJIT()
    {
      Init();
    }

    private void Init()
    {
      IsComplete = false;
      Successes = 0;
      Failures = 0;
    }

    public void Start(IEnumerable<string> assemblies)
    {
      var list = new List<Assembly>();
      foreach (var asm in assemblies)
      {
        try
        {
          var la = Assembly.Load(asm);
          list.Add(la);
        }
        catch (Exception)
        {
        }
      }
      Start(list);
    }

    public void Start(IEnumerable<Assembly> assemblies)
    {
      this.Init();
      var jitter = new Thread(() =>
        {
          try // keep thread from throwing an unhandled exception
          {
            foreach (var asm in assemblies)
            {
              if (_StopFullJitFlag)
                return;
              foreach (var type in asm.GetTypes())
              {
                foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly |
                              BindingFlags.NonPublic | BindingFlags.Public |
                              BindingFlags.Instance | BindingFlags.Static))
                {
                  if (_StopFullJitFlag)
                    return;
                  if (!method.IsAbstract && !method.IsGenericMethod && !method.ContainsGenericParameters)
                  {
                    try
                    {
                      System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
                      Successes++;
                    }
                    catch (Exception)
                    {
                      // keep chuggin
                      Failures++;
                    }
                  }
                }
              }
            }
          }
          catch (Exception)
          {
            Failures++;
          }
          IsComplete = true;
        }) { Priority = ThreadPriority.Lowest };
      jitter.Start();
    }

    private bool _StopFullJitFlag = false;

    public void SignalStop()
    {
      _StopFullJitFlag = true;
    }

    public bool IsComplete { get; private set; }

    public int Successes { get; private set; }

    public int Failures { get; private set; }
  }
}
