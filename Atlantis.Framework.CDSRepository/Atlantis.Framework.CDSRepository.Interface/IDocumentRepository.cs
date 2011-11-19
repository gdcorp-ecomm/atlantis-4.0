namespace Atlantis.Framework.CDSRepository.Interface
{
  public interface IDocumentRepository
  {
    string GetDocument(string query);
    bool Exists(string query);
  }
}
