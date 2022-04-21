namespace ApplicationFaces.Models
{
    public class Statistic
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Confidence { get; set; } = 0;
    }
}
