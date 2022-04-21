using ApplicationFaces.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", GetPeople);

static async Task<string> GetPeople(IConfiguration configuration)
{
    var subscriptionKey = configuration.GetSection("AppSettings").GetSection("AzureSubscriptionKey").Value;
    var endpoint = configuration.GetSection("AppSettings").GetSection("AzureEndPoint").Value;
    var peopleJsonUrl = configuration.GetSection("AppSettings").GetSection("PeopleJsonUrl").Value;
    var websiteUrl = configuration.GetSection("AppSettings").GetSection("WebsiteUrl").Value;

    var personService = new PersonService(subscriptionKey, endpoint, peopleJsonUrl, websiteUrl);

    var people = await personService.GetPeople();

    return JsonConvert.SerializeObject(people);
};

app.Run();
