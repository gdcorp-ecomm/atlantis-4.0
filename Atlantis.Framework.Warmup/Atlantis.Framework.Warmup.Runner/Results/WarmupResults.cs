namespace Atlantis.Framework.Warmup.Runner.Results
{
  using System;
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  [DataContract(IsReference = false, Name = "WarmupResults", Namespace = "")]
  public class WarmupResults : ICloneable
  {
    private List<WarmupResult> _warmupResults;
    [DataMember]
    public List<WarmupResult> Warmups
    {
      get
      {
        return this._warmupResults ?? (this._warmupResults = new List<WarmupResult>());
      }
      set { this._warmupResults = value; }
    }

    [DataMember]
    public int ProcessId { get; set; }

    [DataMember]
    public string Server { get; set; }

    private WarmupSummary _summary;
    [DataMember]
    public WarmupSummary Summary
    {
      get
      {
        return this._summary ?? (this._summary = new WarmupSummary());
      }
      set { this._summary = value; }
    }

    public object Clone()
    {
      var ths = new WarmupResults
        {
          ProcessId = this.ProcessId,
          Server = this.Server,
          _summary = this._summary == null ? null : (WarmupSummary)this._summary.Clone(),
          _warmupResults = this._warmupResults == null ? null : this._warmupResults.ConvertAll( o=> (WarmupResult)o.Clone() )
        };
      return ths;
    }
  }
}
