using System.Collections.Generic;

namespace Atlantis.Framework.CDS.Tokenizer.Interfaces
{
  public interface ITokenizerStrategy
  {
    string Process(List<string> tokens);
  }
}
