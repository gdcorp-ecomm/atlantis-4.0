using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes

{
  public class DotOrgDotAu : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0,           new int[] { 2470, 2472, 2474, 2476, 2478, 2480, 2482, 2484, 2486, 2488 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6,       new int[] { 2505, 2507, 2509, 2511, 2513, 2515, 2517, 2519, 2521, 2523 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21,     new int[] { 2525, 2527, 2529, 2531, 2533, 2535, 2537, 2539, 2541, 2543 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50,    new int[] { 2545, 2547, 2549, 2551, 2553, 2555, 2557, 2559, 2561, 2563 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101,  new int[] { 2565, 2567, 2569, 2571, 2573, 2575, 2577, 2579, 2581, 2583 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201,  new int[] { 2585, 2587, 2589, 2591, 2593, 2595, 2597, 2599, 2602, 2604 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0,           new int[] { 2490, 2491, 2492, 2493, 2494, 2495, 2496, 2497, 2498, 2499});
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6,       new int[] { 2606, 2607, 2608, 2609, 2610, 2611, 2612, 2613, 2614, 2615});
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21,     new int[] { 2616, 2617, 2618, 2619, 2620, 2621, 2622, 2623, 2624, 2625});
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50,    new int[] { 2626, 2627, 2628, 2629, 2630, 2631, 2632, 2633, 2634, 2635});
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101,  new int[] { 2636, 2637, 2638, 2639, 2640, 2641, 2642, 2643, 2644, 2645});
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201,  new int[] { 2646, 2647, 2648, 2649, 2650, 2651, 2652, 2653, 2654, 2655});
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0,           new int[] { 2471, 2473, 2475, 2477, 2479, 2481, 2483, 2485, 2487, 2489});
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6,       new int[] { 2506, 2508, 2510, 2512, 2514, 2516, 2518, 2520, 2522, 2524});
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21,     new int[] { 2526, 2528, 2530, 2532, 2534, 2536, 2538, 2540, 2542, 2544});
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50,    new int[] { 2546, 2548, 2550, 2552, 2554, 2556, 2558, 2560, 2562, 2564});
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101,  new int[] { 2566, 2568, 2570, 2572, 2574, 2576, 2578, 2580, 2582, 2584});
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201,  new int[] { 2586, 2588, 2590, 2592, 2594, 2596, 2598, 2600, 2603, 2605});

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "ORG.AU"; }
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
