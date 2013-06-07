namespace Atlantis.Framework.Warmup.Runner
{
  using System.IO;
  using System.Runtime.Serialization.Json;
  using System.Text;

  internal static class ObjectExtensions
  {
    public static string ToJson(this object value)
    {
      string json;
      var jsSerializer = new DataContractJsonSerializer(value.GetType());
      using (var ms = new MemoryStream())
      {
        jsSerializer.WriteObject(ms, value);
        json = Encoding.Default.GetString(ms.ToArray());
        ms.Close();
      }
      return json;
    }
  }
}
