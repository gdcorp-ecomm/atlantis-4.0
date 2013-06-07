namespace Atlantis.Framework.Warmup.Runner.Results
{
  using System;
  using System.Runtime.Serialization;

  [DataContract(IsReference = false, Namespace = "")]
  public class WarmupSummary : ICloneable
  {
    [DataMember]
    public int Total { get; set; }
    [DataMember]
    public int Successful { get; set; }
    [DataMember]
    public int Failed { get; set; }
    [DataMember]
    public int Ignored { get; set; }

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
