using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class LinkTokenizer : ITokenizerStrategy
  {
    ILinkProvider links;

    public LinkTokenizer()
    {
      links = HttpProviderContainer.Instance.Resolve<ILinkProvider>();
    }

    public string Process(List<string> tokens)
    {
      string url = string.Empty;
      switch (tokens[LinkToken.LINK_TYPE])
      {
        case "full":
          url = links.GetFullUrl(tokens[LinkToken.URL], QueryParamMode.CommonParameters, tokens[LinkToken.QUERYSTRING].Split('|'));
          break;
        default:
          url = links.GetUrl(tokens[LinkToken.LINK_TYPE].ToUpper(), tokens[LinkToken.URL], QueryParamMode.CommonParameters, bool.Parse(tokens[LinkToken.SECURE]), tokens[LinkToken.QUERYSTRING].Split('|'));
          break;
      }
      return url;
    }
  }
}
