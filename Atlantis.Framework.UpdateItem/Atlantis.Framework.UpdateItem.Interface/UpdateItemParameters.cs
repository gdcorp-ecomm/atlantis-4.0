using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.UpdateItem.Interface
{
    public class UpdateItemParameter
    {
        private string _fieldName;
        private string _fieldValue;

        public string FieldName
        {
            get { return _fieldName; }
        }

        public string FieldValue
        {
            get { return _fieldValue; }
        }

        public override string ToString()
        {
            return FieldName + "," + FieldValue;
            
        }

        public UpdateItemParameter(string fieldName, string fieldValue)
        {
            _fieldName = fieldName;
            _fieldValue = fieldValue;
        }
    }
}
