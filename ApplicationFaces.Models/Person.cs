namespace ApplicationFaces.Models
{
	public class Person
	{
		public Guid Id { get; set; } = new Guid()!;

		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string FullName { get; set; } = string.Empty;

		public string Url { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;

		public string DisplayTitle { get; set; } = string.Empty;

		public PersonImageStatistics ImageStatistics { get; set; } = new PersonImageStatistics()!;
	}
}