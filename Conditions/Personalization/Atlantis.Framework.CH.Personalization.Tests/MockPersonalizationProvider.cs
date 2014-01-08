using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization;
using Atlantis.Framework.CH.Personalization.Tests.Properties;


namespace Atlantis.Framework.CH.Personalization.Tests
{
  public class MockPersonalizationProvider : PersonalizationProvider
  {

    public MockPersonalizationProvider(IProviderContainer container)
      : base(container)
    {
    }

    public override TargetedMessages GetTargetedMessages(string interactionPoint)
    {
      TargetedMessages messages = null;
     
      try
      {
        var targetMessageXml = XDocument.Parse(Resources.MockTMSData);

        if (targetMessageXml.Root != null)
        {
          using (XmlReader reader = targetMessageXml.Root.CreateReader())
          {
            XmlSerializer serializer = new XmlSerializer(typeof(TargetedMessages), targetMessageXml.Root.GetDefaultNamespace().NamespaceName);
            messages = (TargetedMessages)serializer.Deserialize(reader);
            reader.Close();
          }
        }
      }
      catch (Exception)
      {
        Debug.WriteLine("GetTargetMessages Deserialization Failed.");
      }

      return messages;
    }
  }
}
