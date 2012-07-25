using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.CDS.Entities.Common.Interfaces;

namespace Atlantis.Framework.CDS.Entities.GDTV.Widgets
{
  public class CelebVideo : IWidgetModel
  {
    public CelebVideo()
    {
      Videos = new List<Video>();
    }
    public List<Video> Videos { get; set; }
  }

  public class Video
  {
    public string Title { get; set; }
    public List<Version> Versions { get; set; }
    public Video()
    {
      Versions = new List<Version>();
    }
  }

  public class Version
  {
    public string Vid { get; set; }
    public string Name { get; set; }
    public bool Featured { get; set; }        
    public string Description { get; set; }
    public string Twitter { get; set; }
    public string Facebook { get; set; }
    public bool Hot { get; set; }
    public int SurveyId { get; set; }
    public string EndSlate { get; set; }
    public string Poster { get; set; }
    public string Thumb { get; set; }
    public string Caption { get; set; }
    public string EndSlateITC { get; set; }
    public string CiCode { get; set; }
    public bool Primary { get; set; }
  }

}
