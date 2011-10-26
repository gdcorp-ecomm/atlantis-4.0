
namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EmailTemplate
  {
    EmailTemplateType _id;
    string _name, _namespace;

    public EmailTemplateType Id
    { get { return _id; } }

    public string Name
    { get { return _name; } }

    public string Namespace
    { get { return _namespace; } }

    public EmailTemplate(EmailTemplateType id, string name, string nameSpace)
    {
      _id = id;
      _name = name;
      _namespace = nameSpace;
    }
  }
}
