using System;
using MongoDB.Bson;

namespace Atlantis.Framework.CDSRepository.Interface
{
  public interface IDocumentRepository
  {
    string GetDocument(string query);
    string GetDocument(string query, ObjectId objectId, DateTime activeDate);
    bool Exists(string query);
  }
}
