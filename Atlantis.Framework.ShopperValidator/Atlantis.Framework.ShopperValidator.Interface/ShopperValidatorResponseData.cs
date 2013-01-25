using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RuleEngine.Results;
using Atlantis.Framework.ShopperValidator.Interface.ShopperValidation;

namespace Atlantis.Framework.ShopperValidator.Interface
{
  public class ShopperValidatorResponseData: IResponseData
  {
    private readonly AtlantisException _aex;
    public ShopperToValidate ValidatedShopper;
    public IModelResult ValidatedModel;

    public bool IsSuccess
    {
      get;
      private set;
    }

    public ShopperValidatorResponseData(IRuleEngineResult engineResults)
    {
      var modelResults = engineResults.ValidationResults;
      ValidatedModel = modelResults.FirstOrDefault(m => m.ModelId == ModelConstants.MODEL_ID_SHOPPERVALID);
      IsSuccess = engineResults.Status == RuleEngineResultStatus.Valid;
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
        xmlSerializer.Serialize(writer, ValidatedShopper);
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
