using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.RCCUpdatePLCategoryData.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RCCUpdatePLCategoryDataTests tests = new RCCUpdatePLCategoryDataTests();
            tests.TestUpdatePlDataHappyPath();
            tests.TestUpdatePlDataPartialSuccess();
            tests.TestUpdatePlDataNoReseller();
            tests.TestUpdatePlDataNoCategoryId();
        }
    }
}
