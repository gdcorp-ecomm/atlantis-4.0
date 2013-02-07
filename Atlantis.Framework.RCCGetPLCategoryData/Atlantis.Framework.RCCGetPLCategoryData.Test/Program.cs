using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.RCCGetPLCategoryData.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            HappyPathTests happyPathTests = new HappyPathTests();
            happyPathTests.TestSingleItemRequest();
            happyPathTests.TestMultipleItemRequest();

            EdgeConditionTests edgeConditionTests = new EdgeConditionTests();
            edgeConditionTests.TestSingleItemRequestNoPrivateLabelId();
            edgeConditionTests.TestSingleItemRequestNoPlCategoryId();
        }
    }
}
