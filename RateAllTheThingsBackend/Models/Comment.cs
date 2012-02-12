using System;

namespace RateAllTheThingsBackend.Models
{
    public class Comment
    {
        public Int64 Id { get; set; }

        public Int64 BarCodeId { get; set; }

        public string Text { get; set; }
        public Int64 Author { get; set; }

        public string Avatar { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}