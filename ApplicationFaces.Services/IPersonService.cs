using ApplicationFaces.Models;

namespace ApplicationFaces.Services
{
    public interface IPersonService
    {
        Task<List<Person>> GetPeople();
    }
}
