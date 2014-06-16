using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Basket.Interface
{
  /// <summary>
  /// The BasketDeleteRequestData class stores data required to delete
  /// items from cart.
  /// </summary>
  public class BasketDeleteRequestData : RequestData
  {
    #region Constants

    private const string DEFAULTBASKETTYPE = "gdshop";

    #endregion

    #region Fields

    private readonly List<BasketDeleteItemKey> _itemsToDelete;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets basket type.
    /// </summary>
    public string BasketType { get; set; }

    /// <summary>
    /// Gets or sets whether is manager.
    /// </summary>
    public bool IsManager { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the BasketDeleteRequestData class that has
    /// default basket type.
    /// </summary>
    public BasketDeleteRequestData()
    {
      _itemsToDelete = new List<BasketDeleteItemKey>();
      BasketType = DEFAULTBASKETTYPE;
    }

    /// <summary>
    /// Initializes a new instance of the BasketDeleteRequestData class that contains
    /// items to be deleted specified, uses the shopper id specified, and has default
    /// basket type.
    /// </summary>
    public BasketDeleteRequestData(string shopperId, IEnumerable<BasketDeleteItemKey> itemsToDelete) : this()
    {
      ShopperID = shopperId;

      if (itemsToDelete != null)
      {
        _itemsToDelete.AddRange(itemsToDelete);
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified row id and item id to items to be deleted.
    /// </summary>
    /// <param name="rowId">Row id of item to be deleted.</param>
    /// <param name="itemId">Item id of item to be deleted.</param>
    public void AddItem(int rowId, int itemId)
    {
      _itemsToDelete.Add(new BasketDeleteItemKey(rowId, itemId));
    }

    /// <summary>
    /// Adds the specified items to items to be deleted.
    /// </summary>
    /// <param name="itemsToDelete">A collection of items to be deleted.</param>
    public void AddItems(IEnumerable<BasketDeleteItemKey> itemsToDelete)
    {
      _itemsToDelete.AddRange(itemsToDelete);
    }

    /// <summary>
    /// Gets a pipe delimited string of items to be deleted.
    /// </summary>
    /// <returns>A pipe delimited string of items to be deleted.</returns>
    public string GetItemKeysToDelete()
    {
      return string.Join("|", _itemsToDelete.Select(i => i.ToString()).ToArray());
    }

    /// <summary>
    /// Converts the current object to XML and returns the string.
    /// </summary>
    /// <returns>A string represents the XML of current object.</returns>
    public override string ToXML()
    {
      var xelement = new XElement(GetType().FullName);

      if (!string.IsNullOrEmpty(ShopperID))
      {
        xelement.Add(new XAttribute("ShopperId", ShopperID));
      }
      xelement.Add(new XAttribute("BasketType", BasketType));
      xelement.Add(new XAttribute("IsManager", IsManager));
      xelement.Add(new XAttribute("ItemsToDelete", GetItemKeysToDelete()));

      return xelement.ToString(SaveOptions.DisableFormatting);
    }

    #endregion
  }
}
