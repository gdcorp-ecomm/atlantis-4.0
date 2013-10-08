namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiGtld
  {
    int Id { get; set; }

    string Name { get; set; }

    int IsIdn { get; set; }

    string IdnScript { get; set; }

    string EnglishMeaning { get; set; }

    string ALabel { get; set; }

    int GtldSubCategoryId { get; set; }

    int DisplayOrder { get; set; }

    string Comments { get; set; }

    bool HasLeafPage { get; set; }

    bool IsFeatured { get; }

    ActionButtonTypes ActionButtonType { get; set; }
  }
}
