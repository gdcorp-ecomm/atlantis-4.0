using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData
{
  public class XmlValidator
  {
    #region Properties
    private static XmlSchemaSet _accordionSchemas = new XmlSchemaSet();
    private static XmlSchemaSet AccordionSchemas
    {
      get
      {
        if (_accordionSchemas.Count.Equals(0))
        {
          _accordionSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("Accordion.xsd"))));
        }
        return _accordionSchemas;
      }
    }

    private static XmlSchemaSet _contentSchemas = new XmlSchemaSet();
    private static XmlSchemaSet ContentSchemas
    {
      get
      {
        if (_contentSchemas.Count.Equals(0))
        {
          _contentSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("Content.xsd"))));
        }
        return _contentSchemas;
      }
    }

    private static XmlSchemaSet _controlPanelSchemas = new XmlSchemaSet();
    private static XmlSchemaSet ControlPanelSchemas
    {
      get
      {
        if (_controlPanelSchemas.Count.Equals(0))
        {
          _controlPanelSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("ControlPanel.xsd"))));
        }
        return _controlPanelSchemas;
      }
    }

    private static XmlSchemaSet _workspaceLoginSchemas = new XmlSchemaSet();
    private static XmlSchemaSet WorkspaceLoginSchemas
    {
      get
      {
        if (_workspaceLoginSchemas.Count.Equals(0))
        {
          _workspaceLoginSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("WorkspaceLogin.xsd"))));
        }
        return _workspaceLoginSchemas;
      }
    }

    private static XmlSchemaSet _linkUrlSchemas = new XmlSchemaSet();
    private static XmlSchemaSet LinkUrlSchemas
    {
      get
      {
        if (_linkUrlSchemas.Count.Equals(0))
        {
          _linkUrlSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("LinkUrl.xsd"))));
        }
        return _linkUrlSchemas;
      }
    }
    #endregion

    #region Public Methods
    public static bool ValidateAccordionXml(XDocument accordionXml, out string errorMsg)
    {
      return ValidateXml(AccordionSchemas, accordionXml, out errorMsg);
    }
    public static bool ValidateContentXml(XDocument contentXml, out string errorMsg)
    {
      return ValidateXml(ContentSchemas, contentXml, out errorMsg);
    }
    public static bool ValidateControlPanelXml(XDocument controlPanelXml, out string errorMsg)
    {
      return ValidateXml(ControlPanelSchemas, controlPanelXml, out errorMsg);
    }
    public static bool ValidateLinkXml(XDocument linkXml, out string errorMsg)
    {
      return ValidateXml(LinkUrlSchemas, linkXml, out errorMsg);
    }
    public static bool ValidateWorkspaceLoginXml(XDocument workspaceLoginXml, out string errorMsg)
    {
      return ValidateXml(WorkspaceLoginSchemas, workspaceLoginXml, out errorMsg);
    }
    #endregion

    #region Private Methods
    private static bool ValidateXml(XmlSchemaSet xmlSchemas, XDocument xml, out string errorMsg)
    {
      bool isValid = false;
      string msg = string.Empty;

      xml.Validate(xmlSchemas,(obj, err) =>
        {
          msg = err.Message;
        });

      errorMsg = msg;
      if (string.IsNullOrWhiteSpace(msg))
      {
        isValid = true;
      }

      return isValid;
    }

    private static string GetSchema(string filename)
    {
      string schema = string.Empty;
      string defaultNamespace = "Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData.XSDs";
      string streampath = string.Format("{0}.{1}", defaultNamespace, filename);

      try
      {
        Assembly assembly = Assembly.GetExecutingAssembly();
        StreamReader schemaStreamReader = new StreamReader(assembly.GetManifestResourceStream(streampath));

        if (schemaStreamReader.Peek() != -1)
        {
          schema = schemaStreamReader.ReadToEnd();
        }
      }
      catch (Exception ex)
      {
        string data = string.Format("Streampath: {0}", streampath);
        AtlantisException aex = new AtlantisException("XmlValidator::GetSchema", "0", ex.Message, data, null, null);
        Engine.Engine.LogAtlantisException(aex);
        schema = string.Empty;
      }

      return schema;
    }
    #endregion

  }
}
