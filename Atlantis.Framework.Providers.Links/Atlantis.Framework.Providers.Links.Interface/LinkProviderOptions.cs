using System;

namespace Atlantis.Framework.Providers.Interface.Links
{
  [Flags]
  public enum LinkProviderOptions : uint
  {
    DefaultOptions                                = 0x00000000,
    /// <summary>
    /// QueryString parameters
    /// If no QueryString parameter is specified, then 'QueryStringCommonParameters' is used
    /// </summary>
    //QueryStringCommonParameters                 = 0x00000000,
    QueryStringExplicitParameters                 = 0x00000001,
    QueryStringExplicitWithLocalizationParameters = 0x00000002,
    //QueryStringReserved                         = 0x00000004,
    //QueryStringReserved                         = 0x00000008,

    /// <summary>
    /// Protocol parameters
    /// If no Protocol parameter is specified, then 'ProtocolCurrentRequest' is used
    /// </summary>
    //ProtocolCurrentRequest                      = 0x00000000,
    ProtocolHttp                                  = 0x00000010,
    ProtocolHttps                                 = 0x00000020,
    ProtocolAgnostic                              = 0x00000040,
    //ProtocolReserved                            = 0x00000080,

    // Reserved blocks
    //Reserved000                                   = 0x00000100,
    //Reserved001                                   = 0x00000200,
    //Reserved005                                   = 0x00000400,
    //Reserved006                                   = 0x00000800,

    //Reserved007                                   = 0x00001000,
    //Reserved008                                   = 0x00002000,
    //Reserved009                                   = 0x00004000,
    //Reserved010                                   = 0x00008000,

    //Reserved011                                   = 0x00010000,
    //Reserved012                                   = 0x00020000,
    //Reserved013                                   = 0x00040000,
    //Reserved014                                   = 0x00080000,

    //Reserved015                                   = 0x00100000,
    //Reserved016                                   = 0x00200000,
    //Reserved017                                   = 0x00400000,
    //Reserved018                                   = 0x00800000,

    //Reserved019                                   = 0x01000000,
    //Reserved020                                   = 0x02000000,
    //Reserved021                                   = 0x04000000,
    //Reserved022                                   = 0x08000000,

    //Reserved023                                   = 0x10000000,
    //Reserved024                                   = 0x20000000,
    //Reserved025                                   = 0x40000000,
    //Reserved026                                   = 0x80000000
  }
}
