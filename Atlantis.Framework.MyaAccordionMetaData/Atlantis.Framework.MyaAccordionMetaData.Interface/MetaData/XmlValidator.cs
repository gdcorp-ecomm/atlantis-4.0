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

    private static XmlSchemaSet _accordionSchemas;
    private static XmlSchemaSet _contentSchemas;
    private static XmlSchemaSet _controlPanelSchemas;
    private static XmlSchemaSet _workspaceLoginSchemas;
    private static XmlSchemaSet _linkUrlSchemas;

    #endregion

    #region Constructor
    static XmlValidator()
    {
      _accordionSchemas = new XmlSchemaSet();
      _contentSchemas = new XmlSchemaSet();
      _controlPanelSchemas = new XmlSchemaSet();
      _workspaceLoginSchemas = new XmlSchemaSet();
      _linkUrlSchemas = new XmlSchemaSet();
      _accordionSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("Accordion.xsd"))));
      _contentSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("Content.xsd"))));
      _controlPanelSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("ControlPanel.xsd"))));
      _workspaceLoginSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("WorkspaceLogin.xsd"))));
      _linkUrlSchemas.Add("", XmlReader.Create(new StringReader(GetSchema("LinkUrl.xsd"))));
    }
  
    #endregion

    #region Public Methods
    public static bool ValidateAccordionXml(XDocument accordionXml, out string errorMsg)
    {
      return ValidateXml(_accordionSchemas, accordionXml, out errorMsg);
    }
    public static bool ValidateContentXml(XDocument contentXml, out string errorMsg)
    {
      return ValidateXml(_contentSchemas, contentXml, out errorMsg);
    }
    public static bool ValidateControlPanelXml(XDocument controlPanelXml, out string errorMsg)
    {
      return ValidateXml(_controlPanelSchemas, controlPanelXml, out errorMsg);
    }
    public static bool ValidateLinkXml(XDocument linkXml, out string errorMsg)
    {
      return ValidateXml(_linkUrlSchemas, linkXml, out errorMsg);
    }
    public static bool ValidateWorkspaceLoginXml(XDocument workspaceLoginXml, out string errorMsg)
    {
      return ValidateXml(_workspaceLoginSchemas, workspaceLoginXml, out errorMsg);
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

      //P84389 - Allow/Ignore superfluous attributes in XML - this will allow Data change to precede supporting DLL changes
      if (string.IsNullOrEmpty(msg) || msg.Contains("attribute is not declared"))
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
