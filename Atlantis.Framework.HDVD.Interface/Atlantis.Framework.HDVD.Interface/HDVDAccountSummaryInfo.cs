using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.HDVD.Interface
{

  [DataContract]
  public class HDVDAccountSummaryInfo
  {

    //feature flags, used to help process all the fields in here

    [DataMember]
    public bool IsDedicated { get; set; }

    [DataMember]
    public bool IsVirtualDedicated { get; set; }

    [DataMember]
    public bool HasFTPBackup { get; set; }

    [DataMember]
    public bool IsAssisted { get; set; }

    [DataMember]
    public bool HasMSSQL { get; set; }

    [DataMember]
    public bool IsPlesk { get; set; }

    [DataMember]
    public bool IsSBP { get; set; }

    [DataMember]
    public bool HasPleskPowerPack { get; set; }

    [DataMember]
    public bool HasFirewall { get; set; }

    [DataMember]
    public bool IsInBillingSystem { get; set; }

    [DataMember]
    public bool HasOSUpgradeAvailable { get; set; }

    [DataMember]
    public bool HasUpcomingScheduledOp { get; set; }

    [DataMember]
    public bool NeedsFirewallSetup { get; set; }


    /// <summary>Hostname of this machine.</summary>
    /// 
    [DataMember]
    public string ServerName { get; set; }

    /// <summary>Customer username for this account.</summary>
    /// 
    [DataMember]
    public string Username { get; set; }

    /// <summary>Operating system this account is provisioned with.</summary>
    /// 
    [DataMember]
    public string OperatingSystem { get; set; }

    /// <summary>Dedicated only. The processor used in this machine.</summary>
    /// 
    [DataMember]
    public string Processor { get; set; }

    /// <summary>Amount of RAM available to this physical machine or virtual instance.</summary>
    /// 
    [DataMember]
    public string RAM { get; set; }

    /// <summary>Amount of disk space available for this account.</summary>
    /// 
    [DataMember]
    public string TotalDiskSpace { get; set; }

    /// <summary>Dedicated only. Will be either "None" or "RAID1".</summary>
    /// 
    [DataMember]
    public string RAID { get; set; }

    /// <summary>Dedicated only.</summary>
    /// 
    [DataMember]
    public IList<string> DiskDrives { get; set; }

    /// <summary>Returns true if customer has bandwidth overage protection enabled.</summary>
    /// 
    [DataMember]
    public bool IsBandwidthOverageProtectionEnabled { get; set; }

    /// <summary>Maximum amount (in gigabytes) of bandwidth this account is allowed.</summary>
    /// 
    [DataMember]
    public string BandwidthQuota { get; set; }

    /// <summary>A rounded-up calculation of the bandwidth used by this account.</summary>
    /// 
    [DataMember]
    public string BandwidthQuotaUsed { get; set; }

    /// <summary>The exact amount of bandwidth used by this account.</summary>
    /// 
    [DataMember]
    public string BandwidthQuotaUsedExact { get; set; }

    /// <summary>Friendly formatting of percentage of bandwidth used by this account.</summary>
    /// 
    [DataMember]
    public string BandwidthPercentUsed { get; set; }

    /// <summary>True if account is under the bandwidth quota; false if account is over quota.</summary>
    /// 
    [DataMember]
    public bool BandwidthUnderQuota { get; set; }

    /// <summary>Contains a message if "hasUpcomingScheduledOp" is set to true.</summary>
    /// 
    [DataMember]
    public string ScheduledOperationMessage { get; set; }

    /// <summary>Dedicated only. The model of the hardware firewall.</summary>
    /// 
    [DataMember]
    public string Firewall { get; set; }

    /// <summary>If the customer has a firewall, this is the URL to the admin page.</summary>
    /// 
    [DataMember]
    public string FirewallAdminURL { get; set; }

    /// <summary>Name of the control panel you have (or None).</summary>
    /// 
    [DataMember]
    public string ControlPanelType { get; set; }

    /// <summary>If the customer has a control panel, this is the URL to the admin page.</summary>
    /// 
    [DataMember]
    public string ControlPanelLaunchURL { get; set; }

    /// <summary>Either the #domains or #users; depending on if you have Plesk or SBP.</summary>
    /// 
    [DataMember]
    public string ParallelsLicenseType { get; set; }

    /// <summary>For Plesk, which paid package you have (e.g. Pro Pack).</summary>
    /// 
    [DataMember]
    public string PleskPackage { get; set; }

    /// <summary>Which SMTP relay server the customer account is configured to use.</summary>
    /// 
    [DataMember]
    public string SMTPService { get; set; }

    /// <summary>Customer's primary IP address.</summary>
    /// 
    [DataMember]
    public string PrimaryIPAddress { get; set; }

    /// <summary>Full list of all customer IP addresses.</summary>
    /// 
    [DataMember]
    public IList<string> IPAddressList { get; set; }

    /// <summary>Username used for connecting to the FTP backup server.</summary>
    /// 
    [DataMember]
    public string FTPBackupUsername { get; set; }

    /// <summary>IP address of the FTP backup server.</summary>
    /// 
    [DataMember]
    public string FTPBackupIPAddress { get; set; }

    /// <summary>Amount of disk space the FTP backup server has allocated for this account.</summary>
    /// 
    [DataMember]
    public string FTPBackupDiskQuota { get; set; }

    /// <summary>Amount of disk space that's been used by the account on the FTP backup server.</summary>
    /// 
    [DataMember]
    public string FTPBackupQuotaUsed { get; set; }

    /// <summary>Dedicated only. Microsoft SQL Server version.</summary>
    /// 
    [DataMember]
    public string MSSQLDatabase { get; set; }

    /// <summary>Describes type of assisted plan this account has.</summary>
    /// 
    [DataMember]
    public IList<string> AssistedServicePlan { get; set; }

    /// <summary>Describes backup schedule this account has (if customer has purchased managed backups).</summary>
    /// 
    [DataMember]
    public IList<string> ManagedBackupServices { get; set; }

  }

}
