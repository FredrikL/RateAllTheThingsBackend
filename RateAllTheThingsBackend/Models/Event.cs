using System;

namespace RateAllTheThingsBackend.Models
{
    public class Event
    {
        public Int64? AuthorId { get; set; }
        public Int64? BarCodeId { get; set; }
        public string EventName { get; set; }
        public string Data { get; set; }
        public string Ip { get; set; }
    }
}