using System;

namespace RateAllTheThingsBackend.Models
{
    public class Comment
    {
        public Int64 Id { get; set; }

        public Int64 BarCodeId { get; set; }

        public string Text { get; set; }
        public Int64 Author { get; set; }

        public decimal Rating { get; set; }
        public bool HasRated { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}