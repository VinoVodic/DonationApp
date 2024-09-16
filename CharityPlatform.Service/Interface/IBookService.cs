using CharityPlatform.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Service.Interface
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
    }
}
