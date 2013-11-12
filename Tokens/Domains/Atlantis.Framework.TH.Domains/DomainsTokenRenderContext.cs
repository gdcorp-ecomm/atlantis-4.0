using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Atlantis.Framework.TH.Domains
{
  public class DomainsTokenRenderContext
  {
    public DomainsTokenRenderContext(IProviderContainer providerContainer)
    {
      if (ReferenceEquals(null, providerContainer))
        throw new ArgumentNullException("providerContainer", "providerContainer is null.");

      ProviderContainer = providerContainer;
    }

    private delegate string TokenRenderer(IToken token, IDotTypeProvider provider);

    public IProviderContainer ProviderContainer
    {
      get;
      private set;
    }

    private static string RenderICANNTldsToken(IToken token, IDotTypeProvider provider)
    {
      string rtnVal = string.Empty;

      GrammaticalDelimiterToken cast = token as GrammaticalDelimiterToken;
      if (!ReferenceEquals(null, cast) && !ReferenceEquals(null, provider))
      {
        var icannFeeTlds = provider.GetTLDDataForRegistration.GetCustomTLDsOfferedByGroupName("IcannFeeTLDs");

        if (!ReferenceEquals(null, icannFeeTlds) && 0 < icannFeeTlds.Count)
        {
          string last = icannFeeTlds.Last();
          icannFeeTlds.Remove(last);

          var delimiter = cast.Delimiter;
          delimiter = !string.IsNullOrEmpty(delimiter) ? delimiter : " ";
          var grammatical = cast.GrammaticalDelimiter;
          grammatical = !string.IsNullOrEmpty(grammatical) ? grammatical : delimiter;

          StringBuilder sb = icannFeeTlds.Aggregate(new StringBuilder(), (current, next) => current.Append(next).Append(delimiter));
          if (0 < sb.Length)
          {
            sb.Remove(sb.Length - delimiter.Length, delimiter.Length);
          }
          rtnVal = sb.AppendFormat("{0}{1}", grammatical, last).ToString();
        }
      }

      return rtnVal;
    }

    public bool RenderToken(IToken token)
    {
      bool rtnVal = false;

      XmlToken cast = token as XmlToken;

      if (!ReferenceEquals(null, cast))
      {
        TokenType tokenType;
        IDotTypeProvider dotTypeProvider = null;

        if (!ReferenceEquals(null, cast.TokenData) && Enum.TryParse(cast.TokenData.Name.ToString(), true, out tokenType) && ProviderContainer.TryResolve<IDotTypeProvider>(out dotTypeProvider))
        {
          string result = string.Empty;
          switch (tokenType)
          {
            case TokenType.ICANNTlds:
              result = RenderICANNTldsToken(token, dotTypeProvider);
              break;
          }

          token.TokenResult = result;
          rtnVal = true;
        }
        else
        {
          const string errorMsg = "Unable to retrieve Identity Provider Information.";
          token.TokenResult = string.Empty;
          token.TokenError = errorMsg;
          Engine.Engine.LogAtlantisException(new AtlantisException("RenderToken", 0, errorMsg, cast.RawTokenData));
        }
      }
      
      return rtnVal;
    }
  }
}
