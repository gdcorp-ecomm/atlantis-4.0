using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotXxx : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{ 57911, 57912, 57913, 57914, 57915, 57916, 57917, 57918, 57919, 57920 }),
        new StaticDotTypeTier(6, new int[]{ 57930, 57943, 57949, 57967, 57955, 57973, 57979, 57985, 57991, 57961 }),
        new StaticDotTypeTier(21, new int[]{ 57931, 57944, 57950, 57968, 57956, 57974, 57980, 57986, 57992, 57962 }),
        new StaticDotTypeTier(50, new int[]{ 57933, 57945, 57951, 57969, 57957, 57975, 57981, 57987, 57993, 57963 }),
        new StaticDotTypeTier(101, new int[]{ 57934, 57946, 57952, 57970, 57958, 57976, 57982, 57988, 57994, 57964 }),
        new StaticDotTypeTier(201, new int[]{ 57935, 57947, 57953, 57971, 57959, 57977, 57983, 57989, 57995, 57965 })
      });
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{ 57921, 57922, 57923, 57924, 57925, 57926, 57927, 57928, 57929 }),
        new StaticDotTypeTier(6, new int[]{ 57936, 57922, 57923, 57924, 57925, 57926, 57927, 57928, 57929 }),
        new StaticDotTypeTier(21, new int[]{ 57937, 57922, 57923, 57924, 57925, 57926, 57927, 57928, 57929 }),
        new StaticDotTypeTier(50, new int[]{ 57939, 57922, 57923, 57924, 57925, 57926, 57927, 57928, 57929 }),
        new StaticDotTypeTier(101, new int[]{ 57940, 57922, 57923, 57924, 57925, 57926, 57927, 57928, 57929 }),
        new StaticDotTypeTier(201, new int[]{ 57941, 57922, 57923, 57924, 57925, 57926, 57927, 57928, 57929 })
      });
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{ 67901, 67906, 67907, 67908, 67909, 67916, 67917, 67918, 67919, 67920 }),
        new StaticDotTypeTier(6, new int[]{ 67930, 67943, 67949, 67967, 67955, 67973, 67979, 67985, 67991, 67961 }),
        new StaticDotTypeTier(21, new int[]{ 67931, 67944, 67950, 67968, 67956, 67974, 67980, 67986, 67992, 67962 }),
        new StaticDotTypeTier(50, new int[]{ 67933, 67945, 67951, 67969, 67957, 67975, 67981, 67987, 67993, 67963 }),
        new StaticDotTypeTier(101, new int[]{ 67934, 67946, 67952, 67970, 67958, 67976, 67982, 67988, 67994, 67964 }),
        new StaticDotTypeTier(201, new int[]{ 67935, 67947, 67953, 67971, 67959, 67977, 67983, 67989, 67995, 67965 })
      });
    }

    protected override StaticDotTypeTiers InitializePreRegistrationProductIds()
    {
      return new StaticDotTypeTiers(StaticDotTypeProductIdTypes.PreRegister, new StaticDotTypeTier[] { 
        new StaticDotTypeTier(0, new int[]{ 57905, 57906, 57607, 57908, 57909, 57909, 57909, 57909, 57909, 58051 }),
        new StaticDotTypeTier(6, new int[]{ 57996, 58009, 58015, 58015, 58021, 58021, 58021, 58021, 58021, 58027 }),
        new StaticDotTypeTier(21, new int[]{ 57997, 58010, 58016, 58016, 58022, 58022, 58022, 58022, 58022, 58028 }),
        new StaticDotTypeTier(50, new int[]{ 57999, 58011, 58017, 58017, 58023, 58023, 58023, 58023, 58023, 58029 }),
        new StaticDotTypeTier(101, new int[]{ 58000, 58012, 58018, 58018, 58024, 58024, 58024, 58024, 58024, 58030 }),
        new StaticDotTypeTier(201, new int[]{ 58001, 58013, 58019, 58019, 58025, 58025, 58025, 58025, 58025, 58031 })
      });
    }


    public override string DotType
    {
      get { return "XXX"; }
    }

    public override int MaxPreRegLength
    {
      get { return 10; }
    }

  }
}
