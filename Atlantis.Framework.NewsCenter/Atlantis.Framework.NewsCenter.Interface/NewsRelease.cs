using Atlantis.Framework.Interface;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Atlantis.Framework.NewsCenter.Interface
{
  public class NewsRelease : IComparable<NewsRelease>
  {
    private static readonly Regex replaceInvalidCharsEx = new Regex("[^a-zA-Z0-9-]+", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex replaceMultipleDash = new Regex("--+", RegexOptions.Compiled);

    public string Id { get; private set; }
    public string Content { get; private set; }
    public string Title { get; private set; }
    public string SubTitle { get; private set; }
    public string ContactFirstName { get; private set; }
    public string ContactLastName { get; private set; }
    public string ContactTitle { get; private set; }
    public string ContactEmail { get; private set; }
    public string ContactPhone { get; private set; }
    public string SEOTitle { get; private set; }
    public string SEODescription { get; private set; }
    public string SEOKeywords { get; private set; }
    public string FooterTitle { get; private set; }
    public string FooterText { get; private set; }
    public string UrlPath { get; private set; }
    public string StatusId { get; private set; }
    public string ReleaseDate { get; private set; }
    public string ExternalFeatureLink { get; private set; }

    public bool IsValid { get; private set; }

    private NewsRelease()
    {
    }

    private static string CreateCleanUrlPath(string text)
    {
      string result = text.Replace(" ", "-");
      result = replaceInvalidCharsEx.Replace(result, string.Empty);
      result = replaceMultipleDash.Replace(result, "-");
      return result.ToLowerInvariant();
    }

    private static string GetAttributeValue(XElement element, string attributeName)
    {
      string result = string.Empty;
      XAttribute attribute = element.Attribute(attributeName);
      if (attribute != null)
      {
        result = attribute.Value;
      }
      return result;
    }

    public static NewsRelease FromDataCacheXElement(XElement newsReleaseElement)
    {
      NewsRelease result = new NewsRelease();

      try
      {
        result.Id = newsReleaseElement.Attribute("news_item_id").Value;
        result.Content = newsReleaseElement.Attribute("article_txt").Value;
        result.Title = newsReleaseElement.Attribute("title").Value;
        result.SubTitle = GetAttributeValue(newsReleaseElement, "short_desc");
        result.ContactFirstName = GetAttributeValue(newsReleaseElement, "first_name");
        result.ContactLastName = GetAttributeValue(newsReleaseElement, "last_name");
        result.ContactEmail = GetAttributeValue(newsReleaseElement, "email_addr");
        result.ContactPhone = GetAttributeValue(newsReleaseElement, "phone");
        result.ContactTitle = GetAttributeValue(newsReleaseElement, "contact_title");
        result.SEOTitle = GetAttributeValue(newsReleaseElement, "seo_title");
        if (string.IsNullOrEmpty(result.SEOTitle))
        {
          result.SEOTitle = result.Title;
        }
        result.SEODescription = GetAttributeValue(newsReleaseElement, "seo_description");
        result.SEOKeywords = GetAttributeValue(newsReleaseElement, "seo_keywords");
        result.FooterTitle = GetAttributeValue(newsReleaseElement, "news_footer_desc"); 
        result.FooterText = GetAttributeValue(newsReleaseElement, "news_footer_txt"); 
        result.UrlPath = CreateCleanUrlPath(result.Title);

        result.StatusId = GetAttributeValue(newsReleaseElement, "news_status_id");
        result.ReleaseDate = GetAttributeValue(newsReleaseElement, "release_date");
        result.ExternalFeatureLink = GetAttributeValue(newsReleaseElement, "link");

        result.IsValid = true;
      }
      catch (Exception ex)
      {
        string data = newsReleaseElement != null ? newsReleaseElement.ToString() : "null element";
        AtlantisException aex = new AtlantisException("NewsRelease.FromDataCacheXElement", "0", ex.Message + ex.StackTrace, data, null, null);
        Atlantis.Framework.Engine.Engine.LogAtlantisException(aex);
        result.IsValid = false;
      }
      
      return result;
    }

    public int CompareTo(NewsRelease other)
    {
      int result = -1;
      if (other != null)
      {
        result = Id.CompareTo(other.Id);
      }
      return result;
    }
  }
}
