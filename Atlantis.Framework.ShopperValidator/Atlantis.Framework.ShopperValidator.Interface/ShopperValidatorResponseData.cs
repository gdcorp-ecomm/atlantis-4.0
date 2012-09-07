using System;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;

namespace Atlantis.Framework.ShopperValidator.Interface
{
  public class ShopperValidatorResponseData: IResponseData
  {
    private AtlantisException _aex;
    public ShopperToValidate ValidatedShopper;

    public bool IsSuccess
    {
      get;
      private set;
    }

    public ShopperValidatorResponseData(ShopperToValidate validatedShopper)
    {
      IsSuccess = validatedShopper != null;
      ValidatedShopper = validatedShopper;
    }

    public ShopperValidatorResponseData(AtlantisException aex)
    {
      IsSuccess = false;
      _aex = aex;
    }
    
    public string ToXML()
    {
      var xmlValue = string.Empty;
      try
      {
        var writer = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(ShopperToValidate));
        xmlSerializer.Serialize(writer, this.ValidatedShopper);
        xmlValue = writer.ToString();
      }
      catch (Exception) { }

      return xmlValue;
    }

    public AtlantisException GetException()
    {
      return _aex;
    }
  }
}
