namespace ESU.Monitoring.Controllers.Filtering
{
    public class HostFilteringParameters
    {
        public string Name { get; set; }

        public string Site { get; set; }

        public string Network { get; set; }

        public int Limit { get; set; } = 10;

        public int Offset { get; set; } = 0;

        public override string ToString()
        {
            return $"Name:{this.Name}, Site:{this.Site}, Network:{this.Network}";
        }
    }
}
