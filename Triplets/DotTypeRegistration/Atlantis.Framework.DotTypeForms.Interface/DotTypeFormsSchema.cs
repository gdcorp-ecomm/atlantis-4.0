﻿using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsSchema : IDotTypeFormsSchema
  {
    public IDotTypeFormsForm Form { get; set; }
    public bool IsSuccess { get; set; }

    public DotTypeFormsSchema() {}

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
              var formLabel = formElement.Attribute("label");
              var formDescription = formElement.Attribute("description");
              var formType = formElement.Attribute("type");
              var validationLevel = formElement.Attribute("level");

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
                  FormLabel = formLabel != null ? formLabel.Value : "",
                  FormDescription = formDescription.Value,
                  FormType = formType.Value,
                  ValidationLevel = validationLevel != null ? validationLevel.Value : "tld",
                  FormFieldCollection = new FormFieldCollection()
                };

              dotTypeFormsForm.FormFieldCollection = ParseFormFieldCollection(formElement);

              var fieldCollection = dotTypeFormsForm.FormFieldCollection.FieldCollection;
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

    private IFormFieldCollection ParseFormFieldCollection(XElement formElement)
    {
      var formFieldCollection = new FormFieldCollection();
      var fieldCollectionElement = formElement.Element("fieldcollection");

      if (fieldCollectionElement != null)
      {
        formFieldCollection = new FormFieldCollection
          {
            Label = fieldCollectionElement.Attribute("label") != null ? fieldCollectionElement.Attribute("label").Value : string.Empty,
            ToggleValue = fieldCollectionElement.Attribute("toggle") != null ? fieldCollectionElement.Attribute("toggle").Value : string.Empty,
            ToggleText = fieldCollectionElement.Attribute("toggletext") != null ? fieldCollectionElement.Attribute("toggletext").Value : string.Empty,
            FieldCollection = new List<IDotTypeFormsField>()
          };
      }

      formFieldCollection.FieldCollection = ParseFieldCollection(formElement);

      return formFieldCollection;
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
            FieldDefaultValue = field.Attribute("default") != null ? field.Attribute("default").Value : string.Empty,
            DataSource = field.Attribute("datasource") != null ? field.Attribute("datasource").Value : string.Empty,
            DataSourceMethod = field.Attribute("method") != null ? field.Attribute("method").Value : string.Empty,
            ItemCollection = ParseItemCollection(field),
            DependsCollection = ParseDependsCollection(field)
          };

        fieldCollection.Add(dotTypeFormsField);
      }

      return fieldCollection;
    }

    #region Parse Collection Methods

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

    private IList<IDependsCollection> ParseDependsCollection(XElement parent)
    {
      IList<IDependsCollection> collection = null;

      var collectionElement = parent.Element("dependscollection");
      if (collectionElement != null)
      {
        collection = new List<IDependsCollection>();
        foreach (XElement item in collectionElement.Elements("dependson"))
        {
          var dotTypeFormsItem = new DependsCollection
          {
            FieldName = item.Attribute("fieldname").Value,
            FieldValue = item.Attribute("fieldvalue").Value
          };
          collection.Add(dotTypeFormsItem);
        }
      }

      return collection;
    }

    #endregion Parse Collection Methods
  }
}
