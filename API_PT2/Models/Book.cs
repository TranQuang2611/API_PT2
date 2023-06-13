using System;
using System.Collections.Generic;

namespace API_PT2.Models
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public int? PubId { get; set; }
        public decimal? Price { get; set; }
        public string? Advance { get; set; }
        public string? Royality { get; set; }
        public string? YtdSales { get; set; }
        public string? Notes { get; set; }
        public DateTime? PublishDate { get; set; }

        public virtual Publisher? Pub { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
