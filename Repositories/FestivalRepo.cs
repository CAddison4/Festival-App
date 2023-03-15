using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{
    public class FestivalRepo
    {
        private readonly ConcertContext _db;

        public FestivalRepo()
        {
            _db = new();
        }
        public int GetCurrentFestivalId() 
        {
            int currentFestivalId = _db.Festivals.Where(a => a.IsCurrent).First().FestivalId;
            return currentFestivalId;
        }
    }
}
