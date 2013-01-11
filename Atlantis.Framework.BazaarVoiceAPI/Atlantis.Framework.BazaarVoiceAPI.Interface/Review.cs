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

    public string Id { get; set; }
    public string ModerationStatus { get; set; }
    public string LastModificationTime { get; set; }
    public string IsRatingsOnly { get; set; }
    public string Title { get; set; }
    public string ReviewText { get; set; }
    public string TotalCommentCount { get; set; }
    public string Rating { get; set; }
    public string RatingRange { get; set; }
    public string IsRecommended { get; set; }
    public string TotalFeedbackCount { get; set; }
    public string TotalPositiveFeedbackCount { get; set; }
    public string TotalNegativeFeedbackCount { get; set; }
    public string UserNickname { get; set; }
    public string UserLocation { get; set; }
    public string ContentLocale { get; set; }
    public string SubmissionTime { get; set; }
    public List<Badge> Badges { get; set; }
    public List<Photo> Photos { get; set; }
    public List<Video> Videos { get; set; }
    public string LastModeratedTime { get; set; }
    public string ProductId { get; set; }
    public string AuthorId { get; set; }
    public string IsSyndicated { get; set; }

    public bool IsValid { get; set; }

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

    public static Review FromDataCacheXElement(XElement reviewElement)
    {
      
      Review result = new Review();
      XNamespace dataApiQuery = "http://www.bazaarvoice.com/xs/DataApiQuery/5.3";

      try
      {
        result.Id = getElementAttribute(reviewElement, "id");
        result.ModerationStatus = getElementValue(reviewElement, "ModerationStatus");
        result.LastModificationTime = getElementValue(reviewElement, "LastModificationTime");
        result.IsRatingsOnly = getElementValue(reviewElement, "IsRatingsOnly");
        result.Title = getElementValue(reviewElement, "Title");
        result.ReviewText = getElementValue(reviewElement, "ReviewText");
        result.TotalCommentCount = getElementValue(reviewElement, "TotalCommentCount");
        result.Rating = getElementValue(reviewElement, "Rating");
        result.RatingRange = getElementValue(reviewElement, "RatingRange");
        result.IsRecommended = getElementValue(reviewElement, "IsRecommended");
        result.TotalFeedbackCount = getElementValue(reviewElement, "TotalFeedbackCount");
        result.TotalPositiveFeedbackCount = getElementValue(reviewElement, "TotalPositiveFeedbackCount");
        result.TotalNegativeFeedbackCount = getElementValue(reviewElement, "TotalNegativeFeedbackCount");
        result.UserNickname = getElementValue(reviewElement, "UserNickname");
        result.UserLocation = getElementValue(reviewElement, "UserLocation");
        result.ContentLocale = getElementValue(reviewElement, "ContentLocale");
        result.SubmissionTime = getElementValue(reviewElement, "SubmissionTime");
        result.Badges = (getElementValue(reviewElement, "Badges", null) != null)
                   ? (from b in reviewElement.Element(dataApiQuery + "Badges").Descendants(dataApiQuery + "Badge")
                      select new Badge
                        {
                          Id = getElementAttribute(b, "id"),
                          ContentType = getElementValue(b, "ContentType")
                        }).ToList()
                   : null;
        result.Photos = (getElementValue(reviewElement, "Photos", null) != null)
                   ? (from p in reviewElement.Element(dataApiQuery + "Photos").Descendants(dataApiQuery + "Photo")
                      select new Photo
                        {
                          Id = getElementAttribute(p, "id"),
                          Sizes = (from s in p.Element(dataApiQuery + "Sizes").Descendants(dataApiQuery + "Size")
                                   select new Size
                                     {
                                       Id = getElementAttribute(s, "id"),
                                       Url = getElementAttribute(s, "url")
                                     }).ToList()
                        }).ToList()
                   : null;
        result.Videos = (getElementValue(reviewElement, "Videos", null) != null)
                   ? (from v in reviewElement.Element(dataApiQuery + "Videos").Descendants(dataApiQuery + "Video")
                      select new Video
                        {
                          Caption = getElementValue(v, "Caption"),
                          VideoHost = getElementValue(v, "VideoHost"),
                          VideoId = getElementValue(v, "VideoId"),
                          VideoUrl = getElementValue(v, "VideoUrl"),
                          VideoThumbnailUrl = getElementValue(v, "VideoThumbnailUrl")
                        }).ToList()
                   : null;
        result.LastModeratedTime = getElementValue(reviewElement, "LastModeratedTime");
        result.ProductId = getElementValue(reviewElement, "ProductId");
        result.AuthorId = getElementValue(reviewElement, "AuthorId");
        result.IsSyndicated = getElementValue(reviewElement, "IsSyndicated");

        result.IsValid = true;
      }
      catch (Exception ex)
      {
        string data = reviewElement != null ? reviewElement.ToString() : "null element";
        //AtlantisException aex = new AtlantisException(requestData, "PCRequest2012.RequestHandler", message, data);
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


    public class Badge
    {
      public string Id { get; set; }
      public string ContentType { get; set; }
    }

    public class Photo
    {
      public string Id { get; set; }
      public List<Size> Sizes { get; set; }
    }

    public class Size
    {
      public string Id { get; set; }
      public string Url { get; set; }
    }

    public class Video
    {
      public string Caption { get; set; }
      public string VideoHost { get; set; }
      public string VideoId { get; set; }
      public string VideoUrl { get; set; }
      public string VideoThumbnailUrl { get; set; }
    }

    public static string getElementValue(XElement x, string elementName, string defaultValue = "")
    {
      XNamespace dataApiQuery = "http://www.bazaarvoice.com/xs/DataApiQuery/5.3";
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

    public static string getElementAttribute(XElement x, string attributeName, string defaultValue = null)
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
