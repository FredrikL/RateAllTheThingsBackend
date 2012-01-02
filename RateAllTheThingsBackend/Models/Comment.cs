using System;

namespace RateAllTheThingsBackend.Models
{
    public class Comment
    {
        public string Text { get; set; }
        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}