using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models.Books
{
    public class GetBooksResponse: Collection<GetBooksResponseItem>
    {
        public string Genre { get; set; }
        public int Count { get; set; }
    }

    public class GetBooksResponseItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}
