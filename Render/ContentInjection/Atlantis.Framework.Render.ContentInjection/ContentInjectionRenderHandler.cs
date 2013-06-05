using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.ContentInjection
{
  public class ContentInjectionRenderHandler : IRenderHandler
  {
    private readonly IContentInjectionContext _context;

    private void LogDebugData(string key, string message, IProviderContainer providerContainer)
    {
      IDebugContext debugContext;
      if (providerContainer.TryResolve(out debugContext))
      {
        debugContext.LogDebugTrackingData(key, message);
      }
    }

    private void ProcessContentItems(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      if (_context.IsValid)
      {
        bool contentInjected = false;
        StringBuilder contentBuilder = new StringBuilder(processedRenderContent.Content);

        foreach (IContentInjectionItem contentInjectionItem in _context.ContentInjectionItems)
        {
          if (ProcessContentInjectionItem(contentInjectionItem, providerContainer, contentBuilder, processedRenderContent.Content))
          {
            contentInjected = true;
          }
        }

        if (contentInjected)
        {
          processedRenderContent.OverWriteContent(contentBuilder.ToString());
        }
      }
    }

    private bool ProcessContentInjectionItem(IContentInjectionItem contentInjectionItem, IProviderContainer providerContainer, StringBuilder contentBuilder, string rawContent)
    {
      bool contentInjected = false;

      if (!string.IsNullOrEmpty(contentInjectionItem.InjectionContent))
      {
        MatchCollection placeHolderMatches = contentInjectionItem.PlaceHolderRegex.Matches(rawContent);
        
        contentInjected = HandleContentInjection(contentInjectionItem, placeHolderMatches, contentBuilder);

        if (!contentInjected)
        {
          LogDebugData("ContentInjectionItem-" + contentInjectionItem,
                       "Content not injected, place holder not found. PlaceholderRegex: " + contentInjectionItem.PlaceHolderRegex + ", Position: " + contentInjectionItem.Position.ToString(),
                       providerContainer);
        }
      }

      return contentInjected;
    }

    private bool HandleContentInjection(IContentInjectionItem contentInjectionItem, MatchCollection placeHolderMatches, StringBuilder contentBuilder)
    {
      bool contentInjected = false;

      if (placeHolderMatches.Count > 0)
      {
        contentInjected = true;

        foreach (Match placeHolderMatch in placeHolderMatches)
        {
          switch (contentInjectionItem.Position)
          {
            case ContentInjectionPosition.Before:
              contentBuilder.Replace(placeHolderMatch.Value, contentInjectionItem.InjectionContent + placeHolderMatch.Value);
              break;
            case ContentInjectionPosition.After:
              contentBuilder.Replace(placeHolderMatch.Value, placeHolderMatch.Value + contentInjectionItem.InjectionContent);
              break;
            case ContentInjectionPosition.Replace:
              contentBuilder.Replace(placeHolderMatch.Value, contentInjectionItem.InjectionContent);
              break;
          }
        }
      }

      return contentInjected;
    }

    public void ProcessContent(IProcessedRenderContent processedRenderContent, IProviderContainer providerContainer)
    {
      ProcessContentItems(processedRenderContent, providerContainer);
    }

    public ContentInjectionRenderHandler(IContentInjectionContext contentInjectionContext)
    {
      if (contentInjectionContext == null)
      {
        contentInjectionContext = new NullContentInjectionContext();
      }

      _context = contentInjectionContext;
    }
  }
}