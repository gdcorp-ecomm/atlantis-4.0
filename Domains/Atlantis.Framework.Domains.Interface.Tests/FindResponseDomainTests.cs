using System;
using System.IO;
using System.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.Domains.Interface.Tests
{
  [TestClass]
  public class FindResponseDomainTests
  {
    const string RAW_JSON_RESPONSE = "{ServerName:\"g1twlep001\",DateTime:\"11/5/2013 1:58:19 PM\",SearchPhrase:\"sdf34sdfsdfsdfsdf.menu\",Keys:[],Data:[{Name:\"bundleid\",Data:\"bundle2_0\"}],ExactDomains:[{Extension:\"menu\",DomainName:\"sdf34sdfsdfsdfsdf.menu\",NameWithoutExtension:\"sdf34sdfsdfsdfsdf\",PunyCodeExtension:\"\",PunyCodeName:\"\",PunyCodeNameWithoutExtension:\"\",Keys:[],Data:[{Name:\"isavailable\",Data:\"true\"},{Name:\"availcheckstatus\",Data:\"full\"},{Name:\"isvaliddomain\",Data:\"true\"},{Name:\"preregphase\",Data:[{Name:\"SRA\",Data:[{Name:\"vendorcost\",Data:\"2500\"},{Name:\"internaltier\",Data:\"19\"}]},{Name:\"LR\",Data:[]},{Name:\"GA\",Data:[]}]},{Name:\"database\",Data:\"similar\"},{Name:\"isoingo\",Data:[{Name:\"isoingo\",Data:\"10\"}]},{Name:\"idnscript\",Data:[{Name:\"eng\",Data:\"35\"}]},{Name:\"isidn\",Data:\"false\"},{Name:\"hasleafpage\",Data:\"false\"}]}]}";

    [TestMethod]
    public void LaunchPhaseItemTest()
    {
      var reader = new JsonTextReader(new StringReader(RAW_JSON_RESPONSE));
      var jsonData = new JsonSerializer().Deserialize(reader) as JObject;

      var domainToken = jsonData["ExactDomains"][0];

      var sld = domainToken["NameWithoutExtension"].ToString();
      var tld = domainToken["Extension"].ToString();
      var punyCodeSld = domainToken["PunyCodeNameWithoutExtension"].ToString();
      var punyCodeTld = domainToken["PunyCodeExtension"].ToString();

      var domainAttributeTokens = domainToken["Data"];

      var responseDomain = new FindResponseDomain(sld, tld, punyCodeSld, punyCodeTld, domainAttributeTokens) as IFindResponseDomain;

      Assert.IsTrue(responseDomain.InPreRegPhase);
      Assert.IsTrue(responseDomain.LaunchPhaseItems.Any());

      var sunrise = responseDomain.LaunchPhaseItems.First(p => p.LaunchPhase == LaunchPhases.SunriseA);
      var landursh = responseDomain.LaunchPhaseItems.First(p => p.LaunchPhase == LaunchPhases.Landrush);

      Assert.IsTrue(sunrise.TierId.Value == 19);

      Assert.IsFalse(landursh.TierId.HasValue);
    }
  }
}
