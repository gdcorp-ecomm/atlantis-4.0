using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSA.Interface
{
  [DataContract]
  public class MailFolder
  {
    [DataMember(Name = "system_folder")]
    [XmlElement(ElementName = "system_folder")]// name element in outgoing soap, when strongly type response is used
    public bool SystemFolder { get; set; }

    [DataMember(Name = "folder_num")]
    [XmlElement(ElementName = "folder_num")]
    public int FolderNum { get; set; }

    [DataMember(Name = "folder")]
    [XmlElement(ElementName = "folder")]
    public string Folder { get; set; }

    [DataMember(Name = "display_name")]
    [XmlElement(ElementName = "display_name")]
    public string DisplayName { get; set; }

    /*[DataMember(Name = "user_num")]
     [XmlElement(ElementName = "user_num")]
     public int UserNum { get; set; }

     [DataMember(Name = "uid_validity")]
     [XmlElement(ElementName = "uid_validity")]
     public int UidValidity { get; set; }

     [DataMember(Name = "uid_next")]
     [XmlElement(ElementName = "uid_next")]
     public int UidNext { get; set; }*/

    [DataMember(Name = "exists_count")]
    [XmlElement(ElementName = "exists_count")]
    private int ExistsCount { get; set; }

    [DataMember(Name = "seen_count")]
    [XmlElement(ElementName = "seen_count")]
    private int SeenCount { get; set; }

    [XmlIgnore]
    public int UnseenCount
    {
      get
      {
        int unseenCount = ExistsCount - SeenCount;
        return unseenCount < 0 ? 0 : unseenCount;
      }
    }

    /*[DataMember(Name = "is_dirty")]
    [XmlElement(ElementName = "is_dirty")]
    public bool IsDirty { get; set; }*/
  }
}
