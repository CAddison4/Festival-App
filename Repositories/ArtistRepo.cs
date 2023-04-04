using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{
    public class ArtistRepo
    {
        private readonly ConcertContext _db;
        public ArtistRepo(ConcertContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all the artists for a given festival. If the optional festivalId parameter is null,
        /// the given/selected festival is taken to be the current festival by default.
        /// </summary>
        /// <param name="festivalId"></param>
        /// <returns>A list of artist objects</returns>
        public List<Artist> GetArtists(int? festivalId=null)
        {
            Festival selectedFestival = _db.Festivals.Where(f => (festivalId != null) ? f.FestivalId == festivalId : f.IsCurrent).First();
            List<Artist> artists = _db.Artists.Where(a => a.Festivals.Contains(selectedFestival)).ToList();
            return artists;
        }

        /// <summary>
        /// Gets all artists in the database, regardless of festival
        /// </summary>
        /// <returns>A list of artist objects</returns>
        public List<Artist> GetAllArtists()
        {
            return _db.Artists.ToList();
        }
    }
}