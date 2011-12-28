using System;
using System.Collections.Generic;

namespace RateAllTheThingsBackend.Models
{
    public class BarCode
    {
        public int Id { get; set; }

        public string Format { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Rating { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime CreatedBy { get; set; }
    }
}