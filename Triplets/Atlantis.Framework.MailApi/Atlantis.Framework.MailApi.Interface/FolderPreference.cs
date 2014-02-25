using System.Runtime.Serialization;

namespace Atlantis.Framework.MailApi.Interface
{

  [DataContract]
  public class FolderPreference
  {
    [DataMember(Name = "user_id")]
    public string UserId { get; set; }

    [DataMember(Name = "folder_name")]
    public string FolderName { get; set; }

    [DataMember(Name = "sort_order")]
    public int SortOrder { get; set; }

    [DataMember(Name = "show_all")]
    public bool ShowAll { get; set; }

    [DataMember(Name = "expanded")]
    public bool Expanded { get; set; }

    [DataMember(Name = "newmail_notice")]
    public bool NewMailNotice { get; set; }

    [DataMember(Name = "long_listing")]
    public bool LongListing { get; set; }

    [DataMember(Name = "show_to")]
    public bool ShowTo { get; set; }

    [DataMember(Name = "unread_top")]
    public bool UnreadTop { get; set; }

    [DataMember(Name = "flagged_top")]
    public bool FlaggedTop { get; set; }

    [DataMember(Name = "uidvalidity")]
    public int UidValidity { get; set; }

    [DataMember(Name = "msg_per_page")]
    public int MsgPerPage { get; set; }

    [DataMember(Name = "pref_sender_view")]
    public bool PrefSenderView { get; set; }
  }
}
