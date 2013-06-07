namespace Atlantis.Framework.Warmup.Runner.Results
{
  using System;
  using System.Runtime.Serialization;

  [DataContract(IsReference=false, Name="Result", Namespace="")]
  public class WarmupResult : ICloneable
  {
    public WarmupResult()
    {
    }

    public WarmupResult(bool? success, string name, string errorMsg)
    {
      this.Success = success;
      this.Name = name;
      this.ErrorMsg = errorMsg;
    }

    [DataMember]
    public bool? Success { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string ErrorMsg { get; set; }

    public object Clone()
    {
      var ths = this.MemberwiseClone();
      return ths;
    }
  }
}
