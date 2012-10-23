using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  [DataContract(Name = "templatePlaceHolder")]
  internal class TemplatePlaceHolder : ITemplatePlaceHolder
  {
    [DataMember(Name = "requestKey")]
    public string RequestKey { get; private set; }

    [IgnoreDataMember]
    public string PlaceHolder { get; set; }

    [IgnoreDataMember]
    public ITemplateSource TemplateSource
    {
      get { return TemplateSourceData; }
    }

    [IgnoreDataMember]
    public IDataSource DataSource
    {
      get { return DataSourceData; }
    }

    [DataMember(Name = "templateSource")]
    public TemplateSource TemplateSourceData { get; set; }

    [DataMember(Name = "dataSource")]
    public DataSource DataSourceData { get; set; }

    internal static ITemplatePlaceHolder GetInstance(string placeHolder, string placeHolderData)
    {
      TemplatePlaceHolder templatePlaceHolder = null;

      try
      {
        byte[] templateDataBytes = Encoding.ASCII.GetBytes(placeHolderData);
        using (MemoryStream ms = new MemoryStream(templateDataBytes))
        {
          DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(TemplatePlaceHolder));
          templatePlaceHolder = (TemplatePlaceHolder)dcs.ReadObject(ms);
          templatePlaceHolder.PlaceHolder = placeHolder;
        }
      }
      catch (Exception ex)
      {
        // TODO: Re-throw or do we want to log silent and continue?
      }

      return templatePlaceHolder;
    }
  }
}
