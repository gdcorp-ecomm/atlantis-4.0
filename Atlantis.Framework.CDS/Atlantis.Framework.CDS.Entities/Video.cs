
namespace Atlantis.Framework.CDS.Entities
{
  public class Video
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public Image Thumb { get; set; }
    public Image Poster { get; set; }
    public string Description { get; set; }
    public string Text { get; set; }
    public bool Hot { get; set; }
    public string CiCode { get; set; }
    public bool Default { get; set; }
    public bool Active { get; set; }
  }
}
