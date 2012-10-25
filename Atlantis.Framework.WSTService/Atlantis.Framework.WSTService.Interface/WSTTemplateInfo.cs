namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTTemplateInfo
  {
    public int CategoryId { get; private set; }
    public int TemplateId { get; private set; }
    public string TemplateLocation { get; private set; }
    public string TemplateName { get; private set; }
    public string TemplateThumbnailUrl { get; private set; }
    public string TemplateUrl { get; private set; }

    public WSTTemplateInfo(int categoryId, int templateId, string templateLocation, string templateName, string templateThumbnailUrl, string templateUrl)
    {
      CategoryId = categoryId;
      TemplateId = templateId;
      TemplateLocation = templateLocation;
      TemplateName = templateName;
      TemplateThumbnailUrl = templateThumbnailUrl;
      TemplateUrl = templateUrl;
    }
  }
}
