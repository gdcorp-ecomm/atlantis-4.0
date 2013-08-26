using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.SplitTesting.Tests.Mocks
{
  class MockEngineRequests
  {
    public static int ActiveSplitTests_NoTests = 999;
    public static int ActiveSplitTests_1Tests = 998;
    public static int ActiveSplitTests_3Tests = 997;
    public static int ActiveSplitTests_ThrowsException = 996;
    public static int ActiveSplitTests_1Tests_WithEligibilityRules = 995;
    public static int ActiveSplitTests_1Tests_WithNeverEligibleElgibilityRules = 994;
    public static int ActiveSplitTests_1Tests_WithMalformedEligibilityRules = 993;
    public static int ActiveSplitTests_1Tests_WithTwoEligibilityRules = 992;
 
    
    public static int ActiveSplitTestDetails_A = 989;
    public static int ActiveSplitTestDetails_AB_50_50 = 988;
    public static int ActiveSplitTestDetails_AB_80_20 = 987;
    public static int ActiveSplitTestDetails_AB_100_0 = 986;
    public static int ActiveSplitTestDetails_ThrowsException = 985;


    public static int AppSettingRequest_24hours = 979;

  }
}

