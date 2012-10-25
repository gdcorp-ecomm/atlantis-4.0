namespace Atlantis.Framework.WSTService.Interface
{
  public class WSTTemplateInfo
  {
    public int CategoryId { get; set; }
    public int TemplateId { get; set; }
    public string TemplateLocation { get; set; }
    public string TemplateName { get; set; }
    public string TemplateThumbnailUrl { get; set; }
    public string TemplateUrl { get; set; }

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
