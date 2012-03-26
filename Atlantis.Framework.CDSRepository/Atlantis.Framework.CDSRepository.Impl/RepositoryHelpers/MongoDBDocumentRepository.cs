using System;
using System.Linq;
using Atlantis.Framework.CDSRepository.Interface;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Atlantis.Framework.CDSRepository.Impl.RepositoryHelpers
{
  class MongoDBDocumentRepository : IDocumentRepository
  {

    MongoServer server;
    //MongoCredentials credentials;

    public MongoDBDocumentRepository(string connectionString)
    {
      Init(connectionString);
    }

    public void Init(string connectionString)
    {
      server = MongoServer.Create(connectionString);
    }

    public bool Exists(string query)
    {
      return false;
    }

    public string GetDocument(string query)
    {
      string json = string.Empty;

      MongoDatabase db = server.GetDatabase("cds");
      MongoCollection<BsonDocument> lps = db.GetCollection<BsonDocument>("pages");
      var mongoQuery = Query.EQ("url", query);
      var doc = lps.FindOne(mongoQuery);
      var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
      json = doc.ToJson<MongoDB.Bson.BsonDocument>(jsonWriterSettings);

      if (string.IsNullOrWhiteSpace(json) || json=="null")
      {
        json = @"{'error':'file not found', 'status':'404'}";
      }

      return json;
    }

    public string GetDocument(string query, ObjectId objectId, DateTime activeDate)
    {
      string json = string.Empty;

      MongoDatabase db = server.GetDatabase("cds");
      MongoCollection<BsonDocument> lps = db.GetCollection<BsonDocument>("pages");

      QueryComplete mongoQuery;

      if (objectId == null)
      {
        mongoQuery = Query.And(Query.EQ("url", query), Query.LTE("activeDate", activeDate));
      }
      else
      {
        mongoQuery = Query.EQ("_id", objectId);
      }

      var doc = lps.Find(mongoQuery).SetLimit(1).FirstOrDefault();
      var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
      json = doc.ToJson<MongoDB.Bson.BsonDocument>(jsonWriterSettings);

      if (string.IsNullOrWhiteSpace(json) || json == "null")
      {
        json = @"{'error':'file not found', 'status':'404'}";
      }

      return json;
    }
  }
}
