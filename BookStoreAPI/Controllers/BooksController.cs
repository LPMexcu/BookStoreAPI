using BookStoreAPI.Models;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;
        public BooksController(BooksService booksService) => _booksService = booksService;

        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Get), new {id=newBook.Id}, newBook);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }
            return book;
        }



        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updateBook)
        {
            var book = await _booksService.GetAsync(id);
            if (book is null)
            {
                return NotFound();
            }

            updateBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updateBook);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            { return NotFound(); }
            await _booksService.RemoveAsync(id);
            return NoContent();
        }
    }
}
