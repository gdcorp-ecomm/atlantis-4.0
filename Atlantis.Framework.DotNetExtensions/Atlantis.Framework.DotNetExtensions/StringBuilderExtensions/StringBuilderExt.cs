namespace Atlantis.Framework.DotNetExtensions.StringBuilderExtensions
{
  using System.Text;
  using System.Collections.Generic;

  public static class StringBuilderExt
  {
    public static StringBuilder RemoveExcludingEndIndex(this StringBuilder sb, int startIndex, int endIndex)
    {
      return sb.Remove(startIndex, endIndex - startIndex);
    }

    public static bool StartsWith(this StringBuilder sb, string s)
    {
      return sb.StartsWith(s, 0);
    }

    public static bool StartsWith(this StringBuilder sb, string s, int startIndex)
    {
      return ((sb.Length - startIndex) >= s.Length) && sb._StartsWith(s, startIndex);
    }

    /// <summary>
    /// This function requires that the value s + startIndex does not extend beyond sb's length.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="s"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    private static bool _StartsWith(this StringBuilder sb, string s, int startIndex)
    {
      int sbi, si;
      for ( sbi=startIndex, si=0; si<s.Length; sbi++,si++ )
      {
        if ( sb[sbi] != s[si] )
        {
          return false;
        }
      }
      return true;
    }

    public static int IndexOf(this StringBuilder sb, char value)
    {
      return sb.IndexOf(value, 0);
    }

    public static int IndexOf(this StringBuilder sb, char value, int startIndex)
    {
      for (; startIndex < sb.Length; startIndex++)
      {
        if ( sb[startIndex] == value )
        {
          return startIndex;
        }
      }
      return -1;
    }

    public static int IndexOf(this StringBuilder sb, string value)
    {
      return sb.IndexOf(value, 0);
    }

    public static int IndexOf(this StringBuilder sb, string value, int startIndex)
    {
      for (; startIndex <= (sb.Length - value.Length); startIndex++)
      {
        if ( sb._StartsWith(value, startIndex) )
        {
          return startIndex;
        }
      }
      return -1;
    }

    /// <summary>
    /// Constructs a 'grammatical list'... such as "blue, red, green and orange".  Assuming the input
    /// params are: itemDelim = "," bUseItemDelimOnSecondToLastItem = false, and secondToLastItemDelim = "and", the items 
    /// in itemList will render as follows:
    /// 
    /// - itemList = ["blue", "red"], sb gets " blue and red"
    /// - itemList = ["blue", "red", "green"], sb gets " blue, red and green"
    /// - itemList = ["blue", "red", "green", "purple"], sb gets " blue, red, green and purple"
    /// 
    /// </summary>
    /// <param name="itemList">list of items in the grammatical list</param>
    /// <param name="itemDelim">delimiter used in lists of size 3 or larger for separating all but the last two items</param>
    /// <param name="secondToLastItemDelim">delimiter used in lists of size 2 or larger for separating the last two items</param>
    /// <param name="bUseItemDelimOnSecondToLastItem">generally false.  set to true if you want lists of size > 2 to render like so: "blue, red and green" to be "blue, red, and green"</param>
    /// <param name="sb">string builder to which the resultant grammatical list is added</param>
    public static void GrammaticalListFormatter(this StringBuilder sb, IList<string> itemList, string itemDelim, string secondToLastItemDelim, bool bUseItemDelimOnSecondToLastItem)
    {
      int countMinusOne = itemList.Count - 1;

      // special handling for no strings
      if (countMinusOne < 0)
      {
        return;
      }

      int i = 0;
      int countMinusTwo = countMinusOne - 1;
      for (; i < countMinusOne; i++)
      {
        // add the item
        sb.Append(" ").Append(itemList[i]);

        // if we are the 2nd to last one
        if (i == countMinusTwo)
        {
          // and you requested the odd case of adding the delimiter as long as this isn't a list of size 2
          if (bUseItemDelimOnSecondToLastItem && countMinusOne > 1)
          {
            sb.Append(itemDelim);
          }

          // now tack on the standard second to last delimiter
          sb.Append(" ").Append(secondToLastItemDelim);
        }
        else
        {
          sb.Append(itemDelim);
        }
      }

      // tack on the last one w/the space char
      sb.Append(" ").Append(itemList[i]);
    }
  }
}
