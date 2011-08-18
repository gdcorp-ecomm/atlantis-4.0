using System;
using Atlantis.Framework.OptIn.Interface.Enums;

namespace Atlantis.Framework.OptIn.Interface
{
  public class OptIn
  {
    public OptIn(int optInId, bool optInStatus, string description)
    {
      IsModified = false;
      _optInId = optInId;
      _status = optInStatus;
      OptInDescription = description;
    }

    public OptIn(OptInPublicationTypes optInType, bool optInStatus, string description)
    {
      IsModified = false;
      Type = optInType;
      _status = optInStatus;
      OptInDescription = description;
    }

    //If Id is not present, type should be utilized.  This initial enum 
    // may be revisited to extend to other publication types later.
    public OptInPublicationTypes Type {
      get
      {
        OptInPublicationTypes myEnum;

        try
        {
          myEnum = (OptInPublicationTypes)Enum.Parse(typeof(OptInPublicationTypes), OptInId.ToString());
        }
        catch
        {
          myEnum = OptInPublicationTypes.Unknown;
        }
        
        return myEnum;
      }
      set { OptInId = (int) value; }
    }

    //Enabled or Disabled
    private bool _status;
    public bool Status
    {
      get { return _status; }
      set {
        if (value != _status)
        {
          IsModified = true;
          _status = value;
        }
      }
    }

    public bool IsModified { get; private set; }

    //if not provided, will rely on type to determine OptIn.
    // currently, this is a way to provide additional flexibility 
    // in case the needed publication type is not available in the existing enumeration
    private int _optInId = -1;
    public int OptInId {
      get
      {
        if (_optInId == -1)
        {
          if (Type != OptInPublicationTypes.Unknown)
          {
            _optInId = (int)Type;
          }  
        }
        return _optInId;
      }
      set { _optInId = value; }
    }

    // May not be utilized in application.  Will need to review need for property at later point
    public string OptInDescription { get; set; }
  }
}
