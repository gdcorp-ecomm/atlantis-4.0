﻿using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotAu
{
  public class DotComDotAu : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 1788, 1790, 1792, 1794, 1796, 1798, 1800, 1824, 1826, 1828 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 1896, 1898, 1906, 1908, 1912, 1914, 1916, 1918, 1920, 1922 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 1924, 1926, 1928, 1930, 1932, 1934, 1936, 1938, 1940, 1942 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 1944, 1946, 1948, 1954, 1956, 1958, 1960, 1962, 1964, 1966 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 1968, 1972, 1974, 1976, 1978, 1981, 1983, 1987, 1989, 1991 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 1993, 1995, 1997, 1999, 2073, 2098, 2136, 2138, 2140, 2142 });
      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 1830, 1834, 1835, 1836, 1837, 1838, 1839, 1840, 1844, 1845 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 2154, 2155, 2156, 2157, 2158, 2159, 2160, 2161, 2162, 2163 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 2164, 2165, 2166, 2167, 2168, 2169, 2170, 2171, 2172, 2173 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 2174, 2175, 2176, 2177, 2178, 2179, 2180, 2181, 2182, 2183 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 2184, 2185, 2186, 2187, 2188, 2189, 2190, 2191, 2192, 2193 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 2194, 2195, 2196, 2197, 2198, 2199, 2200, 2205, 2206, 2207 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Transfer, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 1789, 1791, 1793, 1795, 1797, 1799, 1819, 1825, 1827, 1829 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 1897, 1899, 1907, 1909, 1913, 1915, 1917, 1919, 1921, 1923 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 1925, 1927, 1929, 1931, 1933, 1935, 1937, 1939, 1941, 1943 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 1945, 1947, 1949, 1955, 1957, 1959, 1961, 1963, 1965, 1967 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 1969, 1973, 1975, 1977, 1979, 1982, 1984, 1988, 1990, 1992 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 1994, 1996, 1998, 2032, 2081, 2135, 2137, 2139, 2141, 2143 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.AU"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    public override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }

  }
}
