using System;
using System.Collections.Generic;
using System.Globalization;
using Atlantis.Framework.AddItem.Interface;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal class AddToCartItem : Dictionary<string, string>
  {
    private readonly List<AddToCartItem> _childItems = new List<AddToCartItem>(32);
    
    private string GroupId
    {
      get
      {
        string groupId;
        if (!TryGetValue(AddItemAttributes.GroupId, out groupId))
        {
          groupId = string.Empty;
        }
        return groupId;
      }
      set
      {
        this[AddItemAttributes.GroupId] = value;
      }
    }

    public int ProductId { get; private set; }

    public string CustomXml { get; set; }

    public bool IsChild
    {
      get { return (GroupId.Length > 0) && (!HasChildren); }
    }

    public bool HasChildren
    {
      get { return _childItems.Count > 0; }
    }

    public AddToCartItem(int productId, int quantity)
    {
      ProductId = productId;
      this[AddItemAttributes.UnifiedProductId] = productId.ToString(CultureInfo.InvariantCulture);
      this[AddItemAttributes.Quantity] = quantity.ToString(CultureInfo.InvariantCulture);
    }

    public IEnumerator<AddToCartItem> GetChildEnumerator()
    {
      return _childItems.GetEnumerator();
    }

    public void AddChildItem(AddToCartItem childItem)
    {
      if (IsChild)
      {
        throw new ArgumentException("Cannot add child product to product that is already a child product");
      }

      if (childItem.HasChildren)
      {
        throw new ArgumentException("Cannot add a parent product as a child product");
      }

      if (!HasChildren)
      {
        string groupId = Guid.NewGuid().ToString();
        this[AddItemAttributes.ParentGroupId] = groupId;
        GroupId = groupId;
      }

      childItem[AddItemAttributes.GroupId] = GroupId;
      _childItems.Add(childItem);
    }
  }
}
