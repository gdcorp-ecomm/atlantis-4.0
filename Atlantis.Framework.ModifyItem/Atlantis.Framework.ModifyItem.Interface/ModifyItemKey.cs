using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ModifyItem.Interface
{
    public class ModifyItemKey
    {
        private int _rowId;
        private int _itemId;
        private int _newQuantity;
        public int RowId
        {
            get { return _rowId; }
            set { _rowId = value; }
        }

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }

        }
        public int NewQuantity
        {
            get { return _newQuantity; }
            set { _newQuantity = value; }
        }
        public override string ToString()
        {
            return RowId.ToString() + "," + ItemId.ToString()+","+_newQuantity.ToString();
        }

        public ModifyItemKey(int rowId, int itemId,int newQuantity)
        {
            _rowId = rowId;
            _itemId = itemId;
            _newQuantity = newQuantity;
        }
    }
}
