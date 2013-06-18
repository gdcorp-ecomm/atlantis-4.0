using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.CaptchaCode
{
    internal sealed class GreekCharacterSet : CharacterSet
    {
        // Unicode Code points (32-bit integers) of characters to use for Alpha code generation
        private static readonly Int32[] _alphaCodePoints = { 
            0x03B1, // (Alpha)    α
            0x03B2, // (Beta)     β
            0x03B3, // (Gamma)    γ
            0x03B4, // (Delta)    δ
            0x03B5, // (Epsilon)  ε
            0x03B6, // (Zeta)     ζ
            0x03B7, // (Eta)      η
            0x03B8, // (Theta)    θ
            //0x03B9, // (Iota)     ι
            0x03BA, // (Kappa)    κ
            0x03BB, // (Lamda)    λ
            0x03BC, // (Mu)       μ
            0x03BD, // (Nu)       ν 
            0x03BE, // (Xi)       ξ
            0x03BF, // (Omicron)  ο
            0x03C0, // (Pi)       π
            0x03C1, // (Rho)      ρ
            0x03C3, // (Sigma)    σ
            0x03C4, // (Tau)      τ
            0x03C5, // (Upsilon)  υ
            0x03C6, // (Phi)      φ
            0x03C7, // (Chi)      χ
            0x03C8, // (Psi)      ψ
            0x03C9  // (Omega)    ω
        };

        // Unicode Code points (32-bit integers) of characters to use for Numeric code generation
        private static readonly Int32[] _numericCodePoints = { 
            0x0030, // 0
            0x0031, // 1
            0x0032, // 2
            0x0033, // 3
            0x0034, // 4
            0x0035, // 5
            0x0036, // 6
            //0x0037, // 7
            0x0038, // 8
            0x0039  // 9
        };

        // Unicode Code points (32-bit integers) of characters to use for AlphaNumeric code generation
        private static readonly Int32[] _alphanumericCodePoints = { 
            0x03B1, // (Alpha)    α
            0x03B2, // (Beta)     β
            0x03B3, // (Gamma)    γ
            0x03B4, // (Delta)    δ
            0x03B5, // (Epsilon)  ε
            0x03B6, // (Zeta)     ζ
            0x03B7, // (Eta)      η
            0x03B8, // (Theta)    θ
            //0x03B9, // (Iota)     ι
            0x03BA, // (Kappa)    κ
            0x03BB, // (Lamda)    λ
            0x03BC, // (Mu)       μ
            0x03BD, // (Nu)       ν 
            0x03BE, // (Xi)       ξ
            0x03C0, // (Pi)       π
            0x03C1, // (Rho)      ρ
            0x03C3, // (Sigma)    σ
            0x03C4, // (Tau)      τ
            0x03C5, // (Upsilon)  υ
            0x03C6, // (Phi)      φ
            0x03C7, // (Chi)      χ
            0x03C8, // (Psi)      ψ
            0x03C9, // (Omega)    ω
            0x0031, // 1
            0x0032, // 2
            0x0033, // 3
            0x0034, // 4
            0x0035, // 5
            0x0036, // 6
            //0x0037, // 7
            0x0038, // 8
            0x0039  // 9
        };

        // singleton
        private GreekCharacterSet()
            : base(_alphaCodePoints, _numericCodePoints, _alphanumericCodePoints, "LBD_GreekCharacterSet")
        {
        }

        private static readonly GreekCharacterSet _instance = new GreekCharacterSet();

        public static GreekCharacterSet Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
