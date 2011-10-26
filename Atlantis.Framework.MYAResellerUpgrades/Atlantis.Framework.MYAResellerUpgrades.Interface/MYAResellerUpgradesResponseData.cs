using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYAResellerUpgrades.Interface
{
  public class MYAResellerUpgradesResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public List<ResellerUpgrade> ResellerUpgrades { get; set; }

    public MYAResellerUpgradesResponseData(string xml)
    {

    }

    public MYAResellerUpgradesResponseData(List<ResellerUpgrade> resellerUpgrades)
    {
      ResellerUpgrades = resellerUpgrades;
      IsSuccess = true;
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
      var xdoc = new XDocument();
      var productPlans = new XElement("productplans");

      foreach (ResellerUpgrade ru in ResellerUpgrades)
      {
        productPlans.Add(
          new XElement("productplan",
            new XAttribute("productid", ru.ProductId),
            new XAttribute("description", ru.Description)
          )
        );
      }
      
      xdoc.Add(productPlans);
      return xdoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
