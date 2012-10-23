using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  [DataContract(Name = "dataSource")]
  internal class DataSource : IDataSource
  {
    private IDictionary<string, string> _dataSourceDictionary;
    [IgnoreDataMember]
    private IDictionary<string, string> DataSourceDictionary
    {
      get
      {
        if(_dataSourceDictionary == null)
        {
          if(DataSourceOptions != null)
          {
            _dataSourceDictionary = new Dictionary<string, string>(DataSourceOptions.Length);
            foreach (DataSourceOption dataSourceOption in DataSourceOptions)
            {
              _dataSourceDictionary[dataSourceOption.Key] = dataSourceOption.Value;
            }
          }
          else
          {
            _dataSourceDictionary = new Dictionary<string, string>(0);
          }
        }
        return _dataSourceDictionary;
      }
    }

    [DataMember(Name = "defaultDataSourceId", IsRequired = false)]
    public string DefaultDataSourceId { get; set; }

    [DataMember(Name = "dataSourceOptions")]
    public DataSourceOption[] DataSourceOptions { get; set; }

    [DataMember(Name = "providerAssembly", IsRequired = false)]
    public string ProviderAssembly { get; set; }

    [DataMember(Name = "providerType")]
    public string ProviderType { get; private set; }

    public string GetDataSource(string dataSourceKey)
    {
      string dataSourceValue;

      if(!DataSourceDictionary.TryGetValue(dataSourceKey, out dataSourceValue))
      {
        dataSourceValue = DefaultDataSourceId ?? string.Empty;
      }

      return dataSourceValue;
    }
  }
}