using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Atlantis.Framework.HCC.Interface.DomainSettings
{
  [DataContract]
  public class HCCOptionGroup
  {
    [DataMember]
    string _titleField = string.Empty;
    [DataMember]
    string _textField = string.Empty;
    [DataMember]
    int  _helpArticleIDField;
    [DataMember]
    int _listTypeField;

    [DataMember]
    private ReadOnlyCollection<HCCOptionListItem> _optionsListItems;

    [DataMember(Name = "title")]
    public string Title
    {
      get
      {
        return this._titleField;
      }
      set
      {
        this._titleField = value;
      }
    }
    
    [DataMember(Name = "text")]
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

    [DataMember(Name = "hlpid")]
    public int HelpArticleID
    {
      get
      {
        return this._helpArticleIDField;
      }
      set
      {
        this._helpArticleIDField = value;
      }
    }

    [DataMember(Name = "lsttyp")]
    public int ListType
    {
      get
      {
        return this._listTypeField;
      }
      set
      {
        this._listTypeField = value;
      }
    }

    [DataMember(Name = "hccoptlstitems")]
    public ReadOnlyCollection<HCCOptionListItem> HCCOptionListItems
    {
      get
      {
        return this._optionsListItems ?? new List<HCCOptionListItem>().AsReadOnly();
      }
      private set
      {
        _optionsListItems = value;
      }

    }

    public void SetOptionListItems(ReadOnlyCollection<HCCOptionListItem> optionListItems)
    {
      HCCOptionListItems = optionListItems;
    }
  }
}
