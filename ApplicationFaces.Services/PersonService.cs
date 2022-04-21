using ApplicationFaces.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationFaces.Services
{
    public class PersonService : IPersonService
    {
        private readonly string _subscriptionKey;

        private readonly string _endpoint;

        private readonly string _peopleJsonUrl;

        private readonly string _websiteUrl;

        public PersonService(string subscriptionKey, string endpoint, string peopleJsonUrl, string websiteUrl = "")
        {
            _subscriptionKey = subscriptionKey;
            _endpoint = endpoint;
            _peopleJsonUrl = peopleJsonUrl;
            _websiteUrl = websiteUrl;
        }

        public async Task<List<Person>> GetPeople()
        {
            var people = await GetPeopleFromJsonUrl();

            return people;
        }

        private ComputerVisionClient GetClient()
        {
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey)) { Endpoint = _endpoint };

            return client;
        }

        private async Task<List<Person>> GetPeopleFromJsonUrl()
        {
            var people = new List<Person>();

            var client = new HttpClient();

            var response = await client.GetAsync(_peopleJsonUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (results == null)
            {
                return people;
            }

            var clientAzureCognitiveService = GetClient();

            var data = (JArray)results;

            foreach (var item in data)
            {
                var id = Convert.ToString(item["Id"]);
                var firstName = Convert.ToString(item["FirstName"]);
                var lastName = Convert.ToString(item["LastName"]);
                var fullName = Convert.ToString(item["FullName"]);
                var displayTitle = Convert.ToString(item["DisplayTitle"]);
                var imageUrl = Convert.ToString(item["ImageUrl"]);
                var url = Convert.ToString(item["Url"]);

                var person = new Person()
                {
                    Id = !string.IsNullOrWhiteSpace(id) ? new Guid(id) : new Guid(),
                    FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : string.Empty,
                    LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : string.Empty,
                    FullName = !string.IsNullOrWhiteSpace(fullName) ? fullName : string.Empty,
                    DisplayTitle = !string.IsNullOrWhiteSpace(displayTitle) ? displayTitle : string.Empty,
                    ImageUrl = !string.IsNullOrWhiteSpace(imageUrl) ? $"{_websiteUrl}{imageUrl}" : string.Empty,
                    Url = !string.IsNullOrWhiteSpace(url) ? $"{_websiteUrl}{url}" : string.Empty,
                    ImageStatistics = new PersonImageStatistics()
                    {
                        Id = Guid.NewGuid(),
                        PersonId = !string.IsNullOrWhiteSpace(id) ? new Guid(id) : new Guid(),
                        Captions = new List<Statistic>(),
                        Categories = new List<Statistic>(),
                        Tags = new List<Statistic>(),
                        Faces = new List<Statistic>()
                    }
                };

                await Analyze(clientAzureCognitiveService, person);

                people.Add(person);
            }

            return people;
        }

        private async Task Analyze(ComputerVisionClient client, Person person)
        {
            if (string.IsNullOrWhiteSpace(person.ImageUrl))
            {
                return;
            }

            var features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces,
                VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Color,
                VisualFeatureTypes.Objects
            };

            var results = await client.AnalyzeImageAsync(person.ImageUrl, visualFeatures: features);

            foreach (var category in results.Description.Captions)
            {
                person.ImageStatistics.Captions.Add(new Statistic
                {
                    Name = category.Text,
                    Confidence = category.Confidence
                });
            }

            foreach (var category in results.Categories)
            {
                person.ImageStatistics.Categories.Add(new Statistic
                {
                    Name = category.Name,
                    Confidence = category.Score
                });
            }

            foreach (var tag in results.Tags)
            {
                person.ImageStatistics.Tags.Add(new Statistic
                {
                    Name = tag.Name,
                    Confidence = tag.Confidence
                });
            }

            foreach (var face in results.Faces)
            {
                var gender = Convert.ToString(face.Gender);

                person.ImageStatistics.Faces.Add(new Statistic
                {
                    Name = !string.IsNullOrWhiteSpace(gender) ? gender : string.Empty,
                    Description = face.Age.ToString()
                });
            }

            person.ImageStatistics.ImageAccentColor = results.Color.AccentColor;
            person.ImageStatistics.ImageDominantColorBackground = results.Color.DominantColorBackground;
            person.ImageStatistics.ImageDominantColorForeground = results.Color.DominantColorForeground;
            person.ImageStatistics.ImageDominantColors = string.Join(", ", results.Color.DominantColors);
        }
    }
}