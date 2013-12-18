using Atlantis.Framework.Warmup.Runner.TypeHelpers;

namespace Atlantis.Framework.Warmup.Runner
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Reflection;

  using Atlantis.Framework.Interface;
  using Atlantis.Framework.Warmup.Fixtures;
  using Atlantis.Framework.Warmup.Runner.Interfaces;
  using Atlantis.Framework.Warmup.Runner.Results;

  public sealed class WarmupRunner : IWarmupRun
  {
    public WarmupRunner()
    {
      this.AutoLogResults = true;
      this.LookForWarmupClassesIfNoneProvided = false;
    }

    #region Properties


    public IWarmupSetup Setup { get; set; }

    public IList<Type> WarmupClasses { get; set; }

    private WarmupResults _Results;
    public WarmupResults Results
    {
      get
      {
        return this._Results ?? (this._Results = new WarmupResults());
      }
    }

    public bool AutoLogResults { get; set; }

    public bool LookForWarmupClassesIfNoneProvided { get; set; }

    #endregion


    #region public

    public void Warmup()
    {
      try
      {
        this.GetRunnerExecutionInformation();

        var warmupFixtures = this.GetListOfWarmupFixtures();

        foreach (var warmupFixtureTuple in warmupFixtures)
        {
          this.CurrentWarmupFixtureTuple = warmupFixtureTuple;

          var warmupMethods = this.GetWarmupFixtureMethods();
          if (warmupMethods.Count > 0)
          {
            this._Setup();
            this._Warmups(warmupMethods);
            this._Teardown();
          }
        }
      }
      catch (Exception e)
      {
        this.AddResult(false, e.GetType().ToString(), String.Concat("Exception in warmup runner. ", e.Message));
      }

      if (this.AutoLogResults)
      {
        var res = (WarmupResults)this.Results.Clone();
        res.Warmups.RemoveAll(o => o.Success.HasValue && o.Success.Value);
        var atlEx = new AtlantisException("Warmup", String.Empty, res.Summary.Failed > 0 ? "911" : "411",
                                  res.ToJson(),
                                  String.Empty, "Unknown", String.Empty, "127.0.0.1", String.Empty, 0);
        Engine.Engine.LogAtlantisException(atlEx);
      }
    }

    private void _Warmups(List<MethodInfo> methods)
    {
      foreach (var warmup in methods)
      {
        try
        {
          this.RunWarmupMethod(warmup);
        }
        catch (Exception warmupException)
        {
          this.AddResult(false, warmup.Name, "Exception in warmup. " + warmupException.Message);
        }
      }
    }

    private void _Setup()
    {
      try
      {
        var iWarmupFixture = CurrentWarmupFixtureTuple.Item1 as IWarmupFixture;
        if (iWarmupFixture != null)
        {
          iWarmupFixture.SetupWarmup(Setup);
        }
      }
      catch (Exception ex)
      {
        this.AddResult(false, this.CurrentWarmupFixtureTuple.Item2.FullName, "Exception in warmup setup. " + ex.Message);
      }
    }

    private void _Teardown()
    {
      try
      {
        var iWarmupFixture = CurrentWarmupFixtureTuple.Item1 as IWarmupFixture;
        if (iWarmupFixture != null)
        {
          iWarmupFixture.TeardownWarmup();
        }
      }
      catch (Exception ex)
      {
        this.AddResult(false, this.CurrentWarmupFixtureTuple.Item2.FullName, "Exception in warmup teardown. " + ex.Message);
      }
    }

    #endregion


    #region private

    private Tuple<object, Type> CurrentWarmupFixtureTuple { get; set; }

    private List<Tuple<Object, Type>> GetListOfWarmupFixtures()
    {
      // if no classes are supplied, then search for them
      if (this.LookForWarmupClassesIfNoneProvided && this.WarmupClasses.Count == 0)
      {
        var asms = AppDomain.CurrentDomain.GetAssemblies();
        this.WarmupClasses = ClassFinder.GetClasses(
          asms,
          true,
          new List<Type> {typeof (WarmupFixtureAttribute)}
          );
      }

      var testInsts = this.WarmupClasses
        .Where(
            o =>
              Attribute.GetCustomAttribute(o, this._TypeWarmupFixtureAttribute, false) != null
          )
        .Select(
            t => new Tuple<Object, Type>(Activator.CreateInstance(t), t)
          );
      var insts = new List<Tuple<Object, Type>>(testInsts);
      return insts;
    }

    private void GetRunnerExecutionInformation()
    {
      var processid = Process.GetCurrentProcess().Id;
      var serverName = Environment.MachineName;

      this.Results.ProcessId = processid;
      this.Results.Server = serverName;
    }

    private void AddResult(bool? successValue, string testName, string errorMsg)
    {
      this.Results.Warmups.Add(new WarmupResult(successValue, testName, errorMsg));

      switch (successValue)
      {
        case true:
          this.Results.Summary.Successful += 1;
          break;
        case false:
          this.Results.Summary.Failed += 1;
          break;
        default:
          this.Results.Summary.Ignored += 1;
          break;
      }

      this.Results.Summary.Total += 1;

    }

    private readonly Type _TypeWarmupFixtureAttribute = typeof(WarmupFixtureAttribute);
    private readonly Type _TypeWarmupAttribute = typeof(WarmupAttribute);

    private List<MethodInfo> GetWarmupFixtureMethods()
    {
      var warmups = new List<MethodInfo>();
      var currentClassType = this.CurrentWarmupFixtureTuple.Item2;

      foreach (var mi in currentClassType.GetMethods())
      {
        var testAttr = (WarmupAttribute)Attribute.GetCustomAttribute(mi, this._TypeWarmupAttribute);
        if (testAttr != null)
        {
          warmups.Add(mi);
        }
      }

      return warmups;
    }

    private void RunWarmupMethod(MemberInfo currentWarmup)
    {
      try
      {
        var classAttr = (WarmupFixtureAttribute)Attribute.GetCustomAttribute(this.CurrentWarmupFixtureTuple.Item2, this._TypeWarmupFixtureAttribute);
        var ignore = classAttr != null && classAttr.Ignore;
        if (!ignore)
        {
          var attr = (WarmupAttribute)Attribute.GetCustomAttribute(currentWarmup, this._TypeWarmupAttribute);
          ignore = attr != null && attr.Ignore;

        }

        if (ignore)
        {
          this.AddResult(null, currentWarmup.Name, null);
        }
        else
        {
          ((MethodInfo)currentWarmup).Invoke(this.CurrentWarmupFixtureTuple.Item1, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
          this.AddResult(true, currentWarmup.Name, null);
        }

      }
      catch (Exception ex)
      {
        this.AddResult(false, currentWarmup.Name,
                      ex.InnerException != null
                        ? ex.InnerException.Message
                        : ex.Message);
      }
    }

    #endregion


  }

}
