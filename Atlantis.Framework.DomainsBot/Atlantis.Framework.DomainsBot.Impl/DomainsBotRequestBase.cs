using System.Collections.Generic;

using Atlantis.Framework.DomainsBot.Impl.domainsBot;

namespace Atlantis.Framework.DomainsBot.Impl
{
  public abstract class DomainsBotRequestBase
  {

    protected List<Tld> GetTldList(List<string> oRequsetedTldList)
    {
      List<Tld> oTldList = new List<Tld>();
      double factorVal = 1d;

      foreach (string sTLD in oRequsetedTldList)
      {
        Tld oTld = new Tld();
        oTld.Extension = sTLD;
        oTld.Factor = factorVal;
        oTldList.Add(oTld);

        if (oRequsetedTldList.Count == 2)
        {
          factorVal = factorVal / 2;
        }
        else
        {
          if (oRequsetedTldList.Count > 2)
          {
            factorVal = factorVal - (factorVal / (oRequsetedTldList.Count - 1));
          }
        }
      }

      if (oRequsetedTldList.Count == 0)
      {
        Tld oTld = new Tld();
        oTld.Extension = oRequsetedTldList[0];
        oTld.Factor = 1;
        oTldList.Add(oTld);
      }
      return oTldList;
    }
  }
}
