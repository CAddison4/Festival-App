using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.ViewModel
{
        public class IndexPageVM
        {
            public Festival CurrentFestival { get; set; }

            public IEnumerable<Artist> Artists { get; set; }
        }
}
