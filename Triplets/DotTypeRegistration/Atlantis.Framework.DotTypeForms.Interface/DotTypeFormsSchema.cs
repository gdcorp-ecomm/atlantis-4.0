using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsSchema : IDotTypeFormsSchema
  {
    public int TldId { get; set; }
    public string Placement { get; set; }
    public IList<IDotTypeFormsForm> FormCollection { get; set; }
    public bool IsSuccess { get; set; }

    public DotTypeFormsSchema(string responseXml)
    {
      BuildModelFromXml(responseXml);
    }

    void BuildModelFromXml(string responseXml)
    {
      try
      {
        var formsElement = XElement.Parse(responseXml);

        var tldId = formsElement.Attribute("tldid");
        var placement = formsElement.Attribute("placement");

        if (tldId == null || placement == null)
        {
          const string message = "Xml is missing TldId or Placement in response";
          var xmlHeaderException = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0",
                                                          message, responseXml, null, null);
          throw xmlHeaderException;
        }

        TldId = Int32.Parse(tldId.Value);
        Placement = placement.Value;

        var formCollection = new List<IDotTypeFormsForm>(2);
        var formElementList = formsElement.Descendants("form");
        foreach (var formElement in formElementList)
        {
          var formName = formElement.Attribute("name");
          var formDescription = formElement.Attribute("description");
          var formType = formElement.Attribute("type");
          var formGetMethod = formElement.Attribute("getmethod");
          var formSetMethod = formElement.Attribute("setmethod");

          if (formName == null || formDescription == null || formType == null || formGetMethod == null || formSetMethod == null)
          {
            const string message = "Xml with invalid FormName, FormDescription, FormType, FormGetMethod or FormSetMethod";
            var xmlHeaderException = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0",
                                                            message, responseXml, null, null);
            throw xmlHeaderException;
          }

          var dotTypeFormsForm = new DotTypeFormsForm
          {
            FormName = formName.Value,
            FormDescription = formDescription.Value,
            FormType = formType.Value,
            FormGetMethod = formGetMethod.Value,
            FormSetMethod = formSetMethod.Value,
            ValidationRuleCollection = ParseValidationRuleCollection(formElement)
          };

          var fieldCollection = ParseFieldCollection(formElement);
          if (fieldCollection != null && fieldCollection.Count > 0)
          {
            dotTypeFormsForm.FieldCollection = fieldCollection;
          }
          else
          {
            const string message = "Xml is missing FieldCollection";
            var xmlHeaderException = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0",
                                                            message, responseXml, null, null);
            throw xmlHeaderException;
          }

          formCollection.Add(dotTypeFormsForm);
        }

        FormCollection = formCollection;
        IsSuccess = true;
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0", ex.Message + ex.StackTrace, responseXml, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
    }

    private IList<IDotTypeFormsValidationRule> ParseValidationRuleCollection(XElement parent)
    {
      IList<IDotTypeFormsValidationRule> validationRuleCollection = null;

      var validationRuleCollectionElement = parent.Element("validationrulecollection");
      if (validationRuleCollectionElement != null)
      {
        validationRuleCollection = new List<IDotTypeFormsValidationRule>();
        foreach (var validationRule in validationRuleCollectionElement.Elements("validationrule"))
        {
          var dotTypeFormsValidationRule = new DotTypeFormsValidationRule
          {
            ValidationRuleName = validationRule.Attribute("name").Value
          };
          validationRuleCollection.Add(dotTypeFormsValidationRule);
        }
      }

      return validationRuleCollection;
    }

    private IList<IDotTypeFormsField> ParseFieldCollection(XElement parent)
    {
      IList<IDotTypeFormsField> fieldCollection = null;

      var fieldCollectionElement = parent.Element("fieldcollection");
      if (fieldCollectionElement != null)
      {
        fieldCollection = new List<IDotTypeFormsField>();
        foreach (var field in fieldCollectionElement.Elements("field"))
        {
          var dotTypeFormsField = new DotTypeFormsField
          {
            FieldName = field.Attribute("name").Value,
            FieldLabel = field.Attribute("label").Value,
            FieldDescription = field.Attribute("description").Value,
            FieldType = field.Attribute("type").Value,
            ValidationRuleCollection = ParseValidationRuleCollection(field),
            ItemCollection = ParseItemCollection(field)
          };

          fieldCollection.Add(dotTypeFormsField);
        }
      }

      return fieldCollection;
    }

    private IList<IDotTypeFormsItem> ParseItemCollection(XElement parent)
    {
      IList<IDotTypeFormsItem> itemCollection = null;

      var itemCollectionElement = parent.Element("itemcollection");
      if (itemCollectionElement != null)
      {
        itemCollection = new List<IDotTypeFormsItem>();
        foreach (XElement item in itemCollectionElement.Elements("item"))
        {
          var dotTypeFormsItem = new DotTypeFormsItem
          {
            ItemId = item.Attribute("id").Value,
            ItemLabel = item.Attribute("label").Value
          };
          itemCollection.Add(dotTypeFormsItem);
        }
      }

      return itemCollection;
    }
  }
}
