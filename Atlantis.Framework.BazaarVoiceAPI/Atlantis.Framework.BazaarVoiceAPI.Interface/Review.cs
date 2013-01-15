using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BazaarVoiceAPI.Interface
{
  public class Review : IComparable<Review>
  {
    private static readonly Regex replaceInvalidCharsEx = new Regex("[^a-zA-Z0-9-]+", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex replaceMultipleDash = new Regex("--+", RegexOptions.Compiled);

    public string Id { get; private set; }
    public string ModerationStatus { get; private set; }
    public string LastModificationTime { get; private set; }
    public string IsRatingsOnly { get; private set; }
    public string Title { get; private set; }
    public string ReviewText { get; private set; }
    public string TotalCommentCount { get; private set; }
    public string Rating { get; private set; }
    public string RatingRange { get; private set; }
    public string IsRecommended { get; private set; }
    public string TotalFeedbackCount { get; private set; }
    public string TotalPositiveFeedbackCount { get; private set; }
    public string TotalNegativeFeedbackCount { get; private set; }
    public string UserNickname { get; private set; }
    public string UserLocation { get; private set; }
    public string ContentLocale { get; private set; }
    public string SubmissionTime { get; private set; }
    public List<Badge> Badges { get; private set; }
    public List<Photo> Photos { get; private set; }
    public List<Video> Videos { get; private set; }
    public string LastModeratedTime { get; private set; }
    public string ProductId { get; private set; }
    public string AuthorId { get; private set; }
    public string IsSyndicated { get; private set; }
    public static XNamespace dataApiQuery { get; private set; }

    public bool IsValid { get; private set; }

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

    public static Review FromBazaarApiElement(XElement reviewElement, XNamespace dataApiQuery, RequestData requestData)
    {
      Review result = new Review();

      try
      {
        result.Id = GetElementAttribute(reviewElement, "id");
        result.ModerationStatus = GetElementValue(reviewElement, "ModerationStatus");
        result.LastModificationTime = GetElementValue(reviewElement, "LastModificationTime");
        result.IsRatingsOnly = GetElementValue(reviewElement, "IsRatingsOnly");
        result.Title = GetElementValue(reviewElement, "Title");
        result.ReviewText = GetElementValue(reviewElement, "ReviewText");
        result.TotalCommentCount = GetElementValue(reviewElement, "TotalCommentCount");
        result.Rating = GetElementValue(reviewElement, "Rating");
        result.RatingRange = GetElementValue(reviewElement, "RatingRange");
        result.IsRecommended = GetElementValue(reviewElement, "IsRecommended");
        result.TotalFeedbackCount = GetElementValue(reviewElement, "TotalFeedbackCount");
        result.TotalPositiveFeedbackCount = GetElementValue(reviewElement, "TotalPositiveFeedbackCount");
        result.TotalNegativeFeedbackCount = GetElementValue(reviewElement, "TotalNegativeFeedbackCount");
        result.UserNickname = GetElementValue(reviewElement, "UserNickname");
        result.UserLocation = GetElementValue(reviewElement, "UserLocation");
        result.ContentLocale = GetElementValue(reviewElement, "ContentLocale");
        result.SubmissionTime = GetElementValue(reviewElement, "SubmissionTime");
        result.Badges = (GetElementValue(reviewElement, "Badges", null) != null)
                   ? (from b in reviewElement.Element(dataApiQuery + "Badges").Descendants(dataApiQuery + "Badge")
                      select new Badge
                        {
                          Id = GetElementAttribute(b, "id"),
                          ContentType = GetElementValue(b, "ContentType")
                        }).ToList()
                   : null;
        result.Photos = (GetElementValue(reviewElement, "Photos", null) != null)
                   ? (from p in reviewElement.Element(dataApiQuery + "Photos").Descendants(dataApiQuery + "Photo")
                      select new Photo
                        {
                          Id = GetElementAttribute(p, "id"),
                          Sizes = (from s in p.Element(dataApiQuery + "Sizes").Descendants(dataApiQuery + "Size")
                                   select new Size
                                     {
                                       Id = GetElementAttribute(s, "id"),
                                       Url = GetElementAttribute(s, "url")
                                     }).ToList()
                        }).ToList()
                   : null;
        result.Videos = (GetElementValue(reviewElement, "Videos", null) != null)
                   ? (from v in reviewElement.Element(dataApiQuery + "Videos").Descendants(dataApiQuery + "Video")
                      select new Video
                        {
                          Caption = GetElementValue(v, "Caption"),
                          VideoHost = GetElementValue(v, "VideoHost"),
                          VideoId = GetElementValue(v, "VideoId"),
                          VideoUrl = GetElementValue(v, "VideoUrl"),
                          VideoThumbnailUrl = GetElementValue(v, "VideoThumbnailUrl")
                        }).ToList()
                   : null;
        result.LastModeratedTime = GetElementValue(reviewElement, "LastModeratedTime");
        result.ProductId = GetElementValue(reviewElement, "ProductId");
        result.AuthorId = GetElementValue(reviewElement, "AuthorId");
        result.IsSyndicated = GetElementValue(reviewElement, "IsSyndicated");

        result.IsValid = true;
      }
      catch (Exception ex)
      {
        string data = reviewElement != null ? reviewElement.ToString() : "null element";
        AtlantisException aex = new AtlantisException(requestData, "FromBazaarApiElement", "Error with Element", data);
        result.IsValid = false;
      }

      return result;
    }

    public int CompareTo(Review other)
    {
      int result = -1;
      if (other != null)
      {
        result = String.Compare(Id, other.Id, StringComparison.Ordinal);
      }
      return result;
    }

    private static string GetElementValue(XElement x, string elementName, string defaultValue = "")
    {
      var foundElement = x.Element(dataApiQuery + elementName);
      if (foundElement != null)
      {
        return foundElement.Value;
      }
      else
      {
        return defaultValue;
      }
    }

    private static string GetElementAttribute(XElement x, string attributeName, string defaultValue = null)
    {
      var foundAttribute = x.Attribute(attributeName);
      if (foundAttribute != null)
      {
        return foundAttribute.Value;
      }
      else
      {
        return defaultValue;
      }
    }
  }
}
