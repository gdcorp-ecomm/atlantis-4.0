﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsSchema : IDotTypeFormsSchema
  {
    public IDotTypeFormsForm Form { get; set; }
    public bool IsSuccess { get; set; }

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
          var formsElement = XElement.Parse(responseXml);

          var error = formsElement.Name.LocalName;
          if (!error.Equals("error"))
          {
            var form = new DotTypeFormsForm();
            var formElement = formsElement.Element("form");
            if (formElement != null)
            {
              var formName = formElement.Attribute("name");
              var formDescription = formElement.Attribute("description");
              var formGetMethod = formElement.Attribute("getmethod");
              var formSetMethod = formElement.Attribute("setmethod");

              if (formName == null || formDescription == null || formGetMethod == null || formSetMethod == null)
              {
                const string message =
                  "Xml with invalid FormName, FormDescription, FormType, FormGetMethod or FormSetMethod";
                var xmlHeaderException = new AtlantisException("DotTypeFormsSchemaResponseData.BuildModelFromXml", "0",
                                                               message, responseXml, null, null);
                throw xmlHeaderException;
              }

              var dotTypeFormsForm = new DotTypeFormsForm
                                       {
                                         FormName = formName.Value,
                                         FormDescription = formDescription.Value,
                                         FormGetMethod = formGetMethod.Value,
                                         FormSetMethod = formSetMethod.Value
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
      IList<IDotTypeFormsField> fieldCollection = null;

      var fieldCollectionElements = parent.Descendants("fieldcollection");

      fieldCollection = new List<IDotTypeFormsField>();
      foreach (var field in fieldCollectionElements.Elements("field"))
      {
        var dotTypeFormsField = new DotTypeFormsField
        {
          FieldName = field.Attribute("name").Value,
          FieldLabel = field.Attribute("label").Value,
          FieldDescription = field.Attribute("description").Value,
          FieldType = field.Attribute("type").Value,
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
