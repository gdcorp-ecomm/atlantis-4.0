using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EmailRequired
  {
    OrderData _currentOrder;
    private bool _adminFee = false;
    private bool _DBPAdminFee = false;
    private bool _processFee = false;
    private bool _apiAdminFee = false;
    private bool _otherProductIdsExist = false;

    public EmailRequired(OrderData orderData)
    {
      _currentOrder = orderData;
      DetermineEmailsRequired();
    }

    public bool AdminFee
    {
      get { return _adminFee; }
    }
    public bool DBPAdminFee
    {
      get { return _DBPAdminFee; }
    }
    public bool ProcessFee
    {
      get { return _processFee; }
    }
    public bool ApiAdminFee
    {
      get { return _apiAdminFee; }
    }
    public bool OtherProductIdsExist
    {
      get { return _otherProductIdsExist; }
    }

    private void DetermineEmailsRequired()
    {
      if (!_currentOrder.IsRefund)
      {
        HashSet<int> DBPAdminFeeProductIds =
          new HashSet<int>(new [] { ProductIds.DBPAdminFees_10, 
                                       ProductIds.DBPAdminFees_20, 
                                       ProductIds.DBPAdminFees_30 });

        HashSet<int> ProcessFeeProductIds =
          new HashSet<int>(new [] { ProductIds.WebSiteDesignRestartFee, 
                                       ProductIds.TemplateDesignKillFee, 
                                       ProductIds.BrandIdentityDesignCancellationFee,
                                       ProductIds.WebSiteDesignServiceAdditionalProcessing, 
                                       ProductIds.CustomWebServiceMiscellaneousCharge, 
                                       ProductIds.WebDesignCancellation_E,
                                       ProductIds.WebDesignCancellation_D, 
                                       ProductIds.WebDesignCancellation_P  });

        HashSet<string> AdminFeeDeptIds = new HashSet<string>(new string[] { "45", "49", "200045", "31", "32", "21" });

        XmlNodeList itemNodes = _currentOrder.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
        foreach (XmlNode itemNode in itemNodes)
        {
          XmlElement itemElement = itemNode as XmlElement;
          if (itemElement == null)
          {
            continue;
          }

          string deptId = itemElement.GetAttribute("dept_id");
          string productIdString = itemElement.GetAttribute("_product_unifiedproductid");
          int productId = 0;
          if (!int.TryParse(productIdString, out productId))
          {
            productId = 0;
          }

          if (AdminFeeDeptIds.Contains(deptId))
          {
            _adminFee = true;
          }
          if (productId == ProductIds.LogoDesign)
          {
            _adminFee = false;
          }
          if (DBPAdminFeeProductIds.Contains(productId))
          {
            _DBPAdminFee = true;
          }
          if (productId == ProductIds.AdministrativeFeesInvalidWhoIs_25)
          {
            _apiAdminFee = true;
          }
          if (deptId == "31" || ProcessFeeProductIds.Contains(productId))
          {
            _processFee = true;
          }
          else
          {
            if (deptId == "32" || deptId == "21")
            {
              _processFee = true;
              _otherProductIdsExist = true;
            }
            else
            {
              _otherProductIdsExist = true;
            }
          }
        }
      }
    }

  }
}
