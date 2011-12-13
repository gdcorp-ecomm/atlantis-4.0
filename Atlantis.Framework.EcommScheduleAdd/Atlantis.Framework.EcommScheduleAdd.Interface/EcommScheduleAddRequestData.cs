using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommScheduleAdd.Interface
{
    public class EcommScheduleAddRequestData : RequestData
    {
        public DateTime ScheduledDate { get; set; }
        public int ScheduledHour { get; set; }
        public string DiscussionText { get; set; }
        public string RawHour { get; set; }

        public EcommScheduleAddRequestData(
            string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
            DateTime scheduledDate, int scheduledHour, string discussionText, string rawHour)
            : base(shopperId, sourceUrl, orderId, pathway, pageCount)
        {
            ScheduledDate = scheduledDate;
            ScheduledHour = scheduledHour;
            DiscussionText = discussionText;
            RawHour = rawHour;
            RequestTimeout = TimeSpan.FromSeconds(4);
        }

        public override string GetCacheMD5()
        {
            throw new NotImplementedException("EcommScheduleAddRequestData is not a cacheable request.");
        }
    }
}
