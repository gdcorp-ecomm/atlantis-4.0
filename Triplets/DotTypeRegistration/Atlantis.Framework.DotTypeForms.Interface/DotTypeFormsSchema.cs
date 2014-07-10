using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsSchema : IDotTypeFormsSchema
  {
    public IDotTypeFormsForm Form { get; set; }
    public bool IsSuccess { get; set; }

    public DotTypeFormsSchema()
    {
    }

    public DotTypeFormsSchema(string responseXml)
    {
      BuildModelFromXml(responseXml);
    }

    void BuildModelFromXml(string responseXml)
    {
      try
      {
        if (!string.IsNullOrEmpty(responseXml))
        {
          var formElement = XElement.Parse(responseXml);

          var error = formElement.Name.LocalName;
          if (!error.Equals("error"))
          {
            var form = new DotTypeFormsForm();

            var formElementName = formElement.Name.LocalName;

            if (formElementName.Equals("form"))
            {
              var formName = formElement.Attribute("name");
              var formDescription = formElement.Attribute("description");
              var formType = formElement.Attribute("type");

              if (formName == null || formDescription == null || formType == null)
              {
                const string message =
                  "Xml with invalid FormName, FormDescription, FormType";
                var xmlHeaderException = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0",
                                                                message, responseXml, null, null);
                throw xmlHeaderException;
              }

              var dotTypeFormsForm = new DotTypeFormsForm
                {
                  FormName = formName.Value,
                  FormDescription = formDescription.Value,
                  FormType = formType.Value
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
              form = dotTypeFormsForm;
            }

            Form = form;
            IsSuccess = true;
          }
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0", ex.Message + ex.StackTrace, responseXml, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }
    }

    private IList<IDotTypeFormsField> ParseFieldCollection(XElement parent)
    {
      var fieldCollectionElements = parent.Descendants("fieldcollection");

      IList<IDotTypeFormsField> fieldCollection = new List<IDotTypeFormsField>();
      foreach (var field in fieldCollectionElements.Elements("field"))
      {
        var dotTypeFormsField = new DotTypeFormsField
          {
            FieldName = field.Attribute("name").Value,
            FieldLabel = field.Attribute("label").Value,
            FieldDescription = field.Attribute("description").Value,
            FieldType = field.Attribute("type").Value,
            FieldRequired = field.Attribute("required") != null ? field.Attribute("required").Value : "true",
            DataSource = field.Attribute("datasource") != null ? field.Attribute("datasource").Value : string.Empty,
            DataSourceMethod = field.Attribute("method") != null ? field.Attribute("method").Value : string.Empty,
            ItemCollection = ParseItemCollection(field)
          };

        fieldCollection.Add(dotTypeFormsField);
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
