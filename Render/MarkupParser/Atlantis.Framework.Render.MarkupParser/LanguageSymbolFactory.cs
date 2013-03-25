using System;

namespace Atlantis.Framework.Render.MarkupParser
{
  internal class LanguageSymbolFactory
  {
    private const string IF_PRE_PROCESSOR_START = "if(";
    private const string ELSE_IF_PRE_PROCESSOR_START = "elseif(";
    private const string ELSE_PRE_PROCESSOR = "else";
    private const string END_IF_PRE_PROCESSOR = "endif";

    public static MarkupLanguageSymbol GetPreProcessorSymbol(string preProcessorSymbol, string preProcessorPrefix)
    {
      MarkupLanguageSymbol markupLanguageSymbol;

      if (preProcessorSymbol.StartsWith(preProcessorPrefix + IF_PRE_PROCESSOR_START))
      {
        markupLanguageSymbol = MarkupLanguageSymbol.If;
      }
      else if (preProcessorSymbol.StartsWith(preProcessorPrefix + ELSE_IF_PRE_PROCESSOR_START))
      {
        markupLanguageSymbol = MarkupLanguageSymbol.ElseIf;
      }
      else if (preProcessorSymbol.Equals(preProcessorPrefix + ELSE_PRE_PROCESSOR, StringComparison.Ordinal))
      {
        markupLanguageSymbol = MarkupLanguageSymbol.Else;
      }
      else if (preProcessorSymbol.Equals(preProcessorPrefix + END_IF_PRE_PROCESSOR, StringComparison.Ordinal))
      {
        markupLanguageSymbol = MarkupLanguageSymbol.EndIf;
      }
      else
      {
        markupLanguageSymbol = MarkupLanguageSymbol.None;
      }

      return markupLanguageSymbol;
    }
  }
}
