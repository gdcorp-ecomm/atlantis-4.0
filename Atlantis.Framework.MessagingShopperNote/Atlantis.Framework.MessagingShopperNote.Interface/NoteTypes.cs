using System.Collections.Generic;

namespace Atlantis.Framework.MessagingShopperNote.Interface
{
  public class NoteTypes
  {
    #region Properties & Constants

    public const int SHOPPER_NOTE = 0;
    public const int ACCESS_NOTE = 1;
    public const int CUSTOMER_NOTE = 2;

    private static Dictionary<int, string> _noteTypeXmlStrings;
    private static Dictionary<int, string> NoteTypeXmlStrings
    {
      get
      {
        if (_noteTypeXmlStrings == null)
        {
          _noteTypeXmlStrings = new Dictionary<int, string>();
          _noteTypeXmlStrings.Add(NoteTypes.ACCESS_NOTE, "AccessNote");
          _noteTypeXmlStrings.Add(NoteTypes.CUSTOMER_NOTE, "CustomerNote");
          _noteTypeXmlStrings.Add(NoteTypes.SHOPPER_NOTE, "ShopperNote");
        }
        return _noteTypeXmlStrings;
      }
    }
    #endregion

    public static string GetNoteTypeString(int noteType)
    {
      string noteString;
      NoteTypeXmlStrings.TryGetValue(noteType, out noteString);

      return noteString;
    }
  }
}
