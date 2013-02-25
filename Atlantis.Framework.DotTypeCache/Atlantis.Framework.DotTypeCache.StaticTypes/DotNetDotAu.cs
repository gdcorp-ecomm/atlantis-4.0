using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotAu : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0,           new int[] { 2208, 2210, 2212, 2214, 2216, 2218, 2220, 2222, 2224, 2226 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6,       new int[] { 2238, 2240, 2242, 2244, 2246, 2248, 2250, 2252, 2254, 2256 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21,     new int[] { 2258, 2260, 2262, 2264, 2266, 2268, 2270, 2272, 2274, 2276});
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50,    new int[] { 2278, 2280, 2282, 2284, 2286, 2288, 2290, 2292, 2294, 2296});
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101,  new int[] { 2298, 2300, 2311, 2313, 2315, 2317, 2319, 2321, 2364, 2366});
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201,  new int[] { 2368, 2370, 2372, 2374, 2376, 2378, 2380, 2382, 2384, 2386});
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0,           new int[] { 2228, 2229, 2230, 2231, 2232, 2233, 2234, 2235, 2236, 2237 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6,       new int[] { 2388, 2389, 2390, 2391, 2392, 2393, 2394, 2395, 2396, 2397 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21,     new int[] { 2398, 2399, 2400, 2405, 2431, 2432, 2433, 2434, 2435, 2436 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50,    new int[] { 2437, 2438, 2439, 2441, 2442, 2443, 2444, 2447, 2448, 2449 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101,  new int[] { 2450, 2451, 2452, 2453, 2454, 2455, 2456, 2457, 2458, 2459 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201,  new int[] { 2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0,           new int[] { 2209, 2211, 2213, 2215, 2217, 2219, 2221, 2223, 2225, 2227});
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6,       new int[] { 2239, 2241, 2243, 2245, 2247, 2249, 2251, 2253, 2255, 2257});
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21,     new int[] { 2259, 2261, 2263, 2265, 2267, 2269, 2271, 2273, 2275, 2277});
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50,    new int[] { 2279, 2281, 2283, 2285, 2287, 2289, 2291, 2293, 2295, 2297});
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101,  new int[] { 2299, 2306, 2312, 2314, 2316, 2318, 2320, 2330, 2365, 2367});
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201,  new int[] { 2369, 2371, 2373, 2375, 2377, 2379, 2381, 2383, 2385, 2387});

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.AU"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
    }

    public override int MinRenewalLength
    {
      get { return 2; }
    }

    public override int MaxRenewalLength
    {
      get { return 2; }
    }

    public override int MaxTransferLength
    {
      get { return 1; }
    }

    public override int MaxExpiredAuctionRegLength
    {
      get { return 0; }
    }

    public override int MinExpiredAuctionRegLength
    {
      get { return 0; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 27; } //max 90 days out plus 2 year renewal
    }
  }
}
