using System;

namespace Atlantis.Framework.Providers.Interface.Currency
{
  /// <summary>
  /// Options for price formating
  /// </summary>
  [Flags()]
  public enum PriceFormatOptions : int
  {
    /// <summary>
    /// Standard format with decimal, default symbol, and minus for negatives
    /// </summary>
    None = 0,
    /// <summary>
    /// Drops the decimal from the formatted output
    /// </summary>
    DropDecimal = 1,
    /// <summary>
    /// Drops the symbol from the formatted output
    /// </summary>
    DropSymbol = 2,
    /// <summary>
    /// Uses Html encoded symbol. If DropSymbol flag is set, this flag is ignored.
    /// </summary>
    HtmlSymbol = 4,
    /// <summary>
    /// Uses Ascii symbol. If DropSymbol flag is set, this flag is ignored.
    /// </summary>
    AsciiSymbol = 8,
    /// <summary>
    /// Uses parenthesis instead of a minus for negatives
    /// </summary>
    [Obsolete("Currency negative format is set by the culture now.")]
    NegativeParentheses = 16
  }
}
