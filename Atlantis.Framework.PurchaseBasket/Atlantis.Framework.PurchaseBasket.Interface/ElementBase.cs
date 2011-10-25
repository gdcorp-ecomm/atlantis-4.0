using System.Collections.Generic;

namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public abstract class ElementBase : Dictionary<string, string>
  {
    public abstract string ElementName { get; }

    public virtual string GetStringProperty(string key, string defaultValue)
    {
      string result;
      if (!TryGetValue(key, out result))
      {
        result = defaultValue;
      }
      return result;
    }

    public virtual int GetIntProperty(string key, int defaultValue)
    {
      int result;
      string resultText = GetStringProperty(key, string.Empty);
      if ((string.IsNullOrEmpty(resultText)) || (!int.TryParse(resultText, out result)))
      {
        result = defaultValue;
      }
      return result;
    }
  }
}
