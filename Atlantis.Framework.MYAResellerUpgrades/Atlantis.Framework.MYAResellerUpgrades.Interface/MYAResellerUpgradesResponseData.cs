using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MYAResellerUpgrades.Interface
{
  public class MYAResellerUpgradesResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    private List<ResellerUpgrade> _resellerUpgrades;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public List<ResellerUpgrade> ResellerUpgrades
    {
      get { return _resellerUpgrades; }
      set { _resellerUpgrades = value; }
    }

    public MYAResellerUpgradesResponseData(string xml)
    {

    }

    public MYAResellerUpgradesResponseData(List<ResellerUpgrade> resellerUpgrades)
    {
      _resellerUpgrades = resellerUpgrades;
      _success = true;
    }

     public MYAResellerUpgradesResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public MYAResellerUpgradesResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "MYAResellerUpgradesResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      XDocument xdoc = new XDocument();
      XElement productPlans = new XElement("productplans");
      xdoc.Add(productPlans);

      foreach (ResellerUpgrade ru in ResellerUpgrades)
      {
        productPlans.Add(
          new XElement("productplan",
            new XAttribute("productid", ru.ProductId),
            new XAttribute("description", ru.Description)
          )
        );
      }
      return xdoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
