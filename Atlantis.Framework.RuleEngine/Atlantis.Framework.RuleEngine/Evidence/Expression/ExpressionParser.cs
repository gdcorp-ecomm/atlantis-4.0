using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.RuleEngine.Evidence
{
    public static class ExpressionParser
    {
        private static bool IsTwoCharOp(char c)
        {
            return (c == '=' || c == '!' || c == '>' || c == '<');
        }
        private static bool IsOneCharOp(char c)
        {
            return (c == '+' || c == '-' || c == '/' || c == '*' || c == '^');
        }

        public static string[] Tokenize(string exp)
        {
            List<string> tokens = new List<string>();

            bool inTwoCharOp = false;
            bool inStrConst = false;
            StringBuilder tok = new StringBuilder(1024);
            foreach (char c in exp)
            {
                if (!inStrConst && char.IsWhiteSpace(c))
                {
                    if (tok.Length > 0)
                    {
                        tokens.Add(tok.ToString());
                        tok.Clear();
                    }
                }
                else if (c == '"')
                {
                    if (!inStrConst)
                    {
                        if (tok.Length > 0)
                        {
                            tokens.Add(tok.ToString());
                            tok.Clear();
                        }
                        tok.Append(c);
                        inStrConst = true;
                    }
                    else
                    {
                        tok.Append(c);
                        if (tok.Length > 0)
                        {
                            tokens.Add(tok.ToString());
                            tok.Clear();
                        }
                        inStrConst = false;
                    }
                }
                else if (!inStrConst && (c == '(' || c == ')' || IsOneCharOp(c)))
                {
                    if (tok.Length > 0)
                    {
                        tokens.Add(tok.ToString());
                        tok.Clear();
                    }
                    tokens.Add(c.ToString());
                }
                else if (!inStrConst && IsTwoCharOp(c))
                {
                    if (!inTwoCharOp)
                    {
                        if (tok.Length > 0)
                        {
                            tokens.Add(tok.ToString());
                            tok.Clear();
                        }
                        tok.Append(c);
                        inTwoCharOp = true;
                    }
                    else
                    {
                        tok.Append(c);
                        tokens.Add(tok.ToString());
                        tok.Clear();
                        inTwoCharOp = false;
                    }
                }
                else
                {
                    tok.Append(c);
                }
            }

            if (tok.Length > 0) tokens.Add(tok.ToString());

            return tokens.ToArray();
        }
    }
}
