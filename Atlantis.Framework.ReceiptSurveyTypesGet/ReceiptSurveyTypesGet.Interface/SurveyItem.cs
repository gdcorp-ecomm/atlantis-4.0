using System;
namespace Atlantis.Framework.ReceiptSurveyTypesGet.Interface
{
  public class SurveyItem : IComparable
  {
    public string Value { get; set; }
    public string Text { get; set; }
    public string GroupID { get; private set; }
    public bool IsRacingItem { get; private set; }
    public bool IsTVItem { get; private set; }
    private int _random;

    public SurveyItem(string value, string text, string groupId, int random = -1)
    {
      Value = value;
      Text = text;
      GroupID = groupId;
      _random = random;
      SetItemProperties();
    }

    #region Private Methods
    private void SetItemProperties()
    {
      IsRacingItem = GroupID.Equals(SurveyTypeGroups.TVRacing);
      IsTVItem = IsRacingItem || GroupID.Equals(SurveyTypeGroups.TVGeneral)  || GroupID.Equals(SurveyTypeGroups.TVOther);
    }
    #endregion

    #region IComparble
    public int CompareTo(object obj)
    {
      SurveyItem item2 = (SurveyItem)obj;
      int returnValue = 0;
      bool surveyItemContainsOther =  item2.GroupID == SurveyTypeGroups.TVOther || item2.GroupID == SurveyTypeGroups.NonTvOther;

      if (surveyItemContainsOther)
      {
        returnValue = -1;
      }
      else if (this._random < item2._random)
      {
        returnValue = -1;
      }
      else if (this._random > item2._random)
      {
        returnValue = 1;
      }

      return returnValue;
    }
    #endregion
    
    public SurveyItem Clone(int random)
    {
      SurveyItem si = new SurveyItem(this.Value, this.Text, this.GroupID, random);
      return si;
    }
  }
}
