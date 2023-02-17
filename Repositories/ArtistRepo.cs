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

        public List<Artist> GetArtistsAtCurrentFestival()
        {
            Festival currentFestival = _db.Festivals.Where(f => f.IsCurrent).First();
            List<Artist> cfArtists = _db.Artists.Where(a => a.Festivals.Contains(currentFestival)).ToList();
            return cfArtists;
        }

        public List<Artist> GetAllArtists()
        {
            return _db.Artists.ToList();
        }

        //public List<Artist> GetArtists()
        //{


        //    //var artistList = from a in _db.Artists
        //    //                 join f in _db.FestivalArtists on a.ArtistId equals f.FestivalId where a.ArtistId == 
        //    //int currentFestivalId = _db.CurrentFestivals.FirstOrDefault().FestivalId;
        //    //var currentFestivalArtistIds = _db.Artists.Join(_db.Festivals, a => a.ArtistId, f => f.FestivalId, (a, f) => new { a.ArtistId, f.FestivalId})
        //    //                                           .Where(fa => fa.FestivalId == currentFestivalId)
        //    //                                           .Select( l => new { ArtistId = l.ArtistId, FestivalId = l.FestivalId }).ToList();


        //    List<int> artistIds = _db.Artists.Select(a => a.ArtistId).ToList();

        //    //int[,] artistIds = new int[,] { };
        //    var artists = from a in _db.Artists
        //                  from f in _db.Festivals
        //                  where artistIds.Contains(f.FestivalId) && f.FestivalId.Equals(currentFestivalId)
        //                  select a;

        //    //var currentArtistIds = from f in _db.Festivals
        //    //                       where (from a in f.Artists where artistIds )

        //    List < Artist > currentArtists = new List<Artist>();

        //    //opt 1
        //    //foreach (int id in currentFestivalArtistIds){
        //    //    currentArtists.Add(_db.Artists.Where(a => a.ArtistId == id).FirstOrDefault);


        //    //}

        //opt 2
        //IEnumerable < Artist > results = _db.Artists.Where(currentFestivalArtistIds.ToList().Contains(a => a.ArtistId));

        //var festivalArtists = _db.Festivals
        //.Join(_db.FestivalArtists,
        //    f => new { f.Id, ArtistId = f.Id },
        //    fa => new { fa.FestivalId, fa.ArtistId },
        //    (f, fa) => fa)
        //.Join(_db.Artists,
        //    fa => fa.ArtistId,
        //    a => a.Id,
        //    (fa, a) => new { FestivalArtist = fa, Artist = a })
        //.Where(x => x.FestivalArtist.FestivalId == festivalId && x.Artist.Id == artistId)
        //.Select(x => x.Artist)
        //.ToList();

        //            return currentArtists;
        //        }
        //    }
    }
}
//var userRoles = (from u in _context.Users
//join ur in _context.UserRoles on u.Id equals ur.UserId
//                 where u.Id == userId
//                 select ur).ToList();