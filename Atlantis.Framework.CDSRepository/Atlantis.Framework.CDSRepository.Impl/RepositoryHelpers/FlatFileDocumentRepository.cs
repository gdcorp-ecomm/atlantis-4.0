using System.Collections.Generic;
using System.IO;
using System.Linq;
using Atlantis.Framework.CDSRepository.Interface;

namespace Atlantis.Framework.CDSRepository.Impl.RepositoryHelpers
{

  public class FlatFileDocumentRepository : IDocumentRepository
  {
    private string _rootPath;

    private string _fileType = "json";
    public string FileType
    {
      set
      {
        _fileType = value;
      }
    }

    public FlatFileDocumentRepository(string rootPath)
    {
      _rootPath = rootPath;
    }   

    public bool Exists(string query)
    {
      return File.Exists(filePath(query));
    }

    public string GetDocument(string query)
    {
      string json = string.Empty;
      string file = filePath(query);
      if (File.Exists(file))
      {
        json = File.ReadAllText(file);
      }

      return json;
    }

    private string filePath(string query)
    {
      List<string> items = null;
      string path = _rootPath;
      string fileName = string.Empty;

      if (!string.IsNullOrEmpty(query))
      {
        items = query.Split(new char[] { '/' }).ToList();
      }

      if (items != null && items.Count > 0)
      {
        for (int i = 0; i < items.Count; i++)
        {
          path = Path.Combine(path, items[i]);
        }
      }

      path += string.Format(".{0}", _fileType);

      return path;
    }
  }
}
