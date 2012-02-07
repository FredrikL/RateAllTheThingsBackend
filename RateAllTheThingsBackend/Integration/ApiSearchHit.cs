namespace RateAllTheThingsBackend.Integration
{
    public class ApiSearchHit
    {
        public string Name { get; set;}
        public string Manufacturer { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(this.Name) && string.IsNullOrEmpty(this.Manufacturer); }
        }
    }
}