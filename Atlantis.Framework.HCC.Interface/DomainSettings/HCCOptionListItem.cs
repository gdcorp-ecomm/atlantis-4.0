using System.Runtime.Serialization;

namespace Atlantis.Framework.HCC.Interface.DomainSettings
{
  [DataContract]
  public class HCCOptionListItem
  {
    [DataMember]
    int _valueField;
    [DataMember]
    string _textField;
    [DataMember]
    bool _selectedField;

    [DataMember(Name = "val")]
    public int Value
    {
      get
      {
        return this._valueField;
      }
      set
      {
        this._valueField = value;
      }
    }
    
    [DataMember(Name = "txt")]
    public string Text
    {
      get
      {
        return this._textField;
      }
      set
      {
        this._textField = value;
      }
    }
    
    [DataMember(Name = "selected")]
    public bool Selected
    {
      get
      {
        return this._selectedField;
      }
      set
      {
        this._selectedField = value;
      }
    }
  }
    
}
