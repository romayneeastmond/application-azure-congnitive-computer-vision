namespace ApplicationFaces.Models
{
    public class PersonImageStatistics
    {
        public Guid Id { get; set; } = new Guid()!;

        public Guid PersonId { get; set; } = new Guid()!;

        public string ImageAccentColor { get; set; } = string.Empty;

        public string ImageDominantColorBackground { get; set; } = string.Empty;

        public string ImageDominantColorForeground { get; set; } = string.Empty;

        public string ImageDominantColors { get; set; } = string.Empty;

        public List<Statistic> Captions { get; set; } = new List<Statistic>();

        public List<Statistic> Categories { get; set; } = new List<Statistic>();

        public List<Statistic> Tags { get; set; } = new List<Statistic>();

        public List<Statistic> Faces { get; set; } = new List<Statistic>();
    }
}
