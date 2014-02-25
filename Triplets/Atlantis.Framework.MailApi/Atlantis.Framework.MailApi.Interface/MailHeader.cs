using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class MailHeader
  {
    [DataMember(Name = "flagged")]
    [XmlElement(ElementName = "flagged")]
    public bool Flagged { get; set; }

    [DataMember(Name = "preferred")]
    [XmlElement(ElementName = "preferred")]
    public bool Preferred { get; set; }

    [DataMember(Name = "read")]
    [XmlElement(ElementName = "read")]
    public bool Read { get; set; }

    [DataMember(Name = "recallable")]
    [XmlElement(ElementName = "recallable")]
    public bool Recallable { get; set; }

    [DataMember(Name = "header_num")]
    [XmlElement(ElementName = "header_num")]
    public double HeaderNum { get; set; }

    [DataMember(Name = "folder_num")]
    [XmlElement(ElementName = "folder_num")]
    public double FolderNum { get; set; }

    [DataMember(Name = "msg_uid")]
    [XmlElement(ElementName = "msg_uid")]
    public double MsgUid { get; set; }

    [DataMember(Name = "filename")]
    [XmlElement(ElementName = "filename")]
    public string Filename { get; set; }

    [DataMember(Name = "to_fld")]
    [XmlElement(ElementName = "to_fld")]
    public string ToFld { get; set; }

    [DataMember(Name = "to_sort")]
    [XmlElement(ElementName = "to_sort")]
    public string ToSort { get; set; }

    [DataMember(Name = "cc")]
    [XmlElement(ElementName = "cc")]
    public string Cc { get; set; }

    [DataMember(Name = "from_fld")]
    [XmlElement(ElementName = "from_fld")]
    public string FromFld { get; set; }

    [DataMember(Name = "from_sort")]
    [XmlElement(ElementName = "from_sort")]
    public string FromSort { get; set; }

    [DataMember(Name = "replyto")]
    [XmlElement(ElementName = "replyto")]
    public string ReplyTo { get; set; }

    [DataMember(Name = "internal_date")]
    [XmlElement(ElementName = "internal_date")]
    public double InternalDate { get; set; }

    [DataMember(Name = "msg_date")]
    [XmlElement(ElementName = "msg_date")]
    public double MsgDate { get; set; }

    [DataMember(Name = "subject")]
    [XmlElement(ElementName = "subject")]
    public string Subject { get; set; }

    [DataMember(Name = "subject_sort")]
    [XmlElement(ElementName = "subject_sort")]
    public string SubjectSort { get; set; }

    [DataMember(Name = "msg_size")]
    [XmlElement(ElementName = "msg_size")]
    public double MsgSize { get; set; }

    [DataMember(Name = "modified_date")]
    [XmlElement(ElementName = "modified_date")]
    public string ModifiedDate { get; set; }

    [DataMember(Name = "priority")]
    [XmlElement(ElementName = "priority")]
    public int Priority { get; set; }

    [DataMember(Name = "has_attachment")]
    [XmlElement(ElementName = "has_attachment")]
    public bool HasAttachment { get; set; }

    [DataMember(Name = "is_answered")]
    [XmlElement(ElementName = "is_answered")]
    public bool IsAnswered { get; set; }

    [DataMember(Name = "is_draft")]
    [XmlElement(ElementName = "is_draft")]
    public bool IsDraft { get; set; }

    [DataMember(Name = "is_forwarded")]
    [XmlElement(ElementName = "is_forwarded")]
    public bool IsForwarded { get; set; }

    [DataMember(Name = "message_id")]
    [XmlElement(ElementName = "message_id")]
    public string MessageId { get; set; }

    [DataMember(Name = "is_secure")]
    [XmlElement(ElementName = "is_secure")]
    public bool IsSecure { get; set; }

    public bool ShouldSerializeIsSecure()
    {
      return (IsSecure == true);
    }
  }
}
