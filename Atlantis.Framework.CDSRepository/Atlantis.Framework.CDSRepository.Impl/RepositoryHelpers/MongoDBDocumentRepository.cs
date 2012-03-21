using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDSRepository.Interface;
using MongoDB.Driver;
using MongoDB.Bson;
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
      var doc = lps.Find(mongoQuery).SetFields(Fields.Exclude("_id", "pageId", "activeDate")).SetLimit(1).FirstOrDefault();
      //var doc = lps.FindOne(mongoQuery); // this returns json with the ObjectId NOT in proper JSON format
      json = doc.ToJson<MongoDB.Bson.BsonDocument>();

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

      var doc = lps.Find(mongoQuery).SetFields(Fields.Exclude("_id", "pageId", "activeDate")).SetLimit(1).FirstOrDefault();
      json = doc.ToJson<MongoDB.Bson.BsonDocument>();

      if (string.IsNullOrWhiteSpace(json) || json == "null")
      {
        json = @"{'error':'file not found', 'status':'404'}";
      }

      return json;
    }
  }
}
