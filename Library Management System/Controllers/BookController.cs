using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Container = Microsoft.Azure.Cosmos.Container;
using Microsoft.Azure.Cosmos;
using Library_Management_System.Entity;
using Library_Management_System.DTO;    
namespace Library_Management_System.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
    
        public string URI = "https://localhost:8081";
        public string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public string DatabaseName = "LibraryM";
        public string ContainerName = "LibraryContainer";
        public Container container;


        private Container GetContainer()
        {
            CosmosClient cosmosClient = new CosmosClient(URI, PrimaryKey);
            Database database = cosmosClient.GetDatabase(DatabaseName);
            Container container = database.GetContainer(ContainerName);
            return container;
        }
        public BookController()
        {
            container = GetContainer();
        }
        [HttpPost]
     
        public async Task<BookDto> AddBook(BookDto bookDto)
        {
            BookEntity book = new BookEntity();
            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.PublishedDate = bookDto.PublishedDate;
            book.ISBN = bookDto.ISBN;
            book.IsIssued = bookDto.IsIssued;


            book.Id = Guid.NewGuid().ToString();
            book.UId = book.Id;
            book.DocumentType = "Book";
            book.CreatedBy = "Shruti";
            book.CreatedOn = DateTime.Now;
            book.UpdatedBy = "";
            book.UpdatedOn = DateTime.Now;
            book.Version = 1;
            book.Active = true;
            book.Archived = false;

            BookEntity response = await container.CreateItemAsync(book);

          
            BookDto responseDto = new BookDto();
            responseDto.UId = response.UId;
            responseDto.Title = response.Title;
            responseDto.Author = response.Author;
            responseDto.PublishedDate = response.PublishedDate;
            responseDto.ISBN = response.ISBN;
            responseDto.IsIssued = response.IsIssued;
            return responseDto;
        }
        [HttpGet]

        public async Task<BookDto> GetBookByUID(string UId)
        {
            var book = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.UId == UId).FirstOrDefault();

            BookDto bookDto = new BookDto();

            bookDto.UId = book.UId;
            bookDto.Title = book.Title;
            bookDto.Author = book.Author;
            bookDto.PublishedDate = book.PublishedDate;
            bookDto.ISBN = book.ISBN;
            bookDto.IsIssued = book.IsIssued;

            return bookDto;
        }
        [HttpGet]
        public async Task<BookDto> GetBookByName(String Title)
        {
            var book = container.GetItemLinqQueryable<BookDto>(true).Where(q => q.Title == Title).FirstOrDefault();

            BookDto bookDto = new BookDto();
            bookDto.UId = book.UId;
            bookDto.Title = book.Title;
            bookDto.Author = book.Author;
            bookDto.PublishedDate = book.PublishedDate;
            bookDto.ISBN = book.ISBN;
            bookDto.IsIssued = book.IsIssued;
            return bookDto;
        }

        [HttpGet]
        public async Task<List<BookDto>> GetAllBooks()
        {
            var books = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.DocumentType == "Book").ToList();

            List<BookDto> bookDtos = new List<BookDto>();
            foreach (var book in books)
            {
                BookDto bookdto = new BookDto();
                bookdto.UId = book.UId;
                bookdto.Title = book.Title;
                bookdto.Author = book.Author;
                bookdto.PublishedDate = book.PublishedDate;
                bookdto.ISBN = book.ISBN;
                bookdto.IsIssued = book.IsIssued;


                bookDtos.Add(bookdto);
            }
            return bookDtos;


        }

        [HttpGet]
        public async Task<List<BookDto>> GetAllNotIssuedBook()
        {
            var books = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.IsIssued == false).ToList();

            List<BookDto> bookdtos = new List<BookDto>();
            foreach (var book in books)
            {
                BookDto bookDto = new BookDto();
                bookDto.UId = book.UId;
                bookDto.Title = book.Title;
                bookDto.Author = book.Author;
                bookDto.PublishedDate = book.PublishedDate;
                bookDto.ISBN = book.ISBN;
                bookDto.IsIssued = book.IsIssued;

                bookdtos.Add(bookDto);
            }
            return bookdtos;

        }
        [HttpGet]
        public async Task<List<BookDto>> GetAllIssuedBook()
        {
            var books = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.IsIssued == true).ToList();

            List<BookDto> bookdtos = new List<BookDto>();
            foreach (var book in books)
            {
                BookDto bookDto = new BookDto();
                bookDto.UId = book.UId;
                bookDto.Title = book.Title;
                bookDto.Author = book.Author;
                bookDto.PublishedDate = book.PublishedDate;
                bookDto.ISBN = book.ISBN;
                bookDto.IsIssued = book.IsIssued;

                bookdtos.Add(bookDto);
            }
            return bookdtos;

        }
        [HttpPost]


        public async Task<BookDto> UpdateBook(BookDto book)
        {

            var existingBook = container.GetItemLinqQueryable<BookEntity>(true).Where(q => q.UId == book.UId && q.Active == true && q.Archived == false).FirstOrDefault();

            await container.ReplaceItemAsync(existingBook, existingBook.Id);

            existingBook.Id = Guid.NewGuid().ToString();
            existingBook.UpdatedBy = "Shruti";
            existingBook.UpdatedOn = DateTime.Now;
            existingBook.Version = existingBook.Version + 1;
            existingBook.Active = true;
            existingBook.Archived = false;
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.PublishedDate = book.PublishedDate;
            existingBook.ISBN = book.ISBN;
            existingBook.IsIssued = book.IsIssued;
            existingBook = await container.CreateItemAsync(existingBook);


            BookDto reponse = new BookDto();
            reponse.UId = existingBook.UId;
            reponse.Title = existingBook.Title;
            reponse.Author = existingBook.Author;
            reponse.PublishedDate = existingBook.PublishedDate;
            reponse.ISBN = existingBook.ISBN;
            reponse.IsIssued = existingBook.IsIssued;
            return reponse;

        }
    }
}


















