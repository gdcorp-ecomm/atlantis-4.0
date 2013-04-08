using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSA.Interface
{
  [DataContract]
  public class MessageHeader
  {
    [DataMember(Name = "subject")]
    public string Subject { get; set; }

   /* [DataMember(Name = "from")]
    [XmlElement(ElementName = "from")]
    public string From { get; set; }

    [DataMember(Name = "to")]
    [XmlElement(ElementName = "to")]
    public string To { get; set; }*/

    [DataMember(Name = "cc")]
    [XmlElement(ElementName="cc")]
    public string Cc { get; set; }

    /*[DataMember(Name = "bcc")]
    public string Bcc { get; set; }

    [DataMember(Name = "date")]
    public string Date { get; set; }*/

    [DataMember(Name = "flagged")]
    public bool Flagged { get; set; }

    [DataMember(Name = "preferred")]
    public bool Preferred { get; set; }

    [DataMember(Name = "read")]
    public bool Read { get; set; }

    /*[DataMember(Name = "recallabe")]
    public bool Recallabe { get; set; }*/

    [DataMember(Name = "header_num")]
    public double HeaderNum { get; set; }

    [DataMember(Name = "folder_num")]
    public double FolderNum { get; set; }

    [DataMember(Name = "msg_uid")]
    public double MsgUid { get; set; }

    [DataMember(Name = "filename")]
    public string Filename { get; set; }

    [DataMember(Name = "to_fld")]
    public string ToFld { get; set; }

    /*[DataMember(Name = "to_sort")]
    public string ToSort { get; set; }
    */
    [DataMember(Name = "from_fld")]
    public string FromFld { get; set; }
    /*
    [DataMember(Name = "from_sort")]
    public string FromSort { get; set; }*/

    [DataMember(Name = "replyto")]
    public string ReplyTo { get; set; }

    [DataMember(Name = "internal_date")]
    public double InternalDate { get; set; }

    [DataMember(Name = "msg_date")]
    public double MsgDate { get; set; }

  /*  [DataMember(Name = "subject_sort")]
    public string SubjectSort { get; set; }*/

    [DataMember(Name = "msg_size")]
    public double MsgSize { get; set; }

   /* [DataMember(Name = "modified_date")]
    public string ModifiedDate { get; set; }*/

    [DataMember(Name = "priority")]
    public int Priority { get; set; }

    [DataMember(Name = "has_attachment")]
    public bool HasAttachment { get; set; }

    [DataMember(Name = "is_answered")]
    public bool IsAnswered { get; set; }

    [DataMember(Name = "is_draft")]
    public bool IsDraft { get; set; }

    [DataMember(Name = "is_forwarded")]
    public bool IsForwarded { get; set; }

    /*[DataMember(Name = "message_id")]
    public string MessageId { get; set; }*/

    [DataMember(Name = "is_secure")]
    [XmlElement(ElementName = "is_secure")]
    public bool IsSecure { get; set; }
    public bool ShouldSerializeIsSecure()
    {
      return IsSecure;
    }
  }
}
