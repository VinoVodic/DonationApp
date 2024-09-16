using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Domain.Domain
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice {  get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
    }
}
