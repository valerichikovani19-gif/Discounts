using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class AuthorService :IAuthorService 
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<AuthorDto>> GetAllAuthorsAsync(CancellationToken cancellationToken)
        {
            var authors = await _unitOfWork.Authors.GetAllAsync(cancellationToken);
            //mapingi
            return authors.Select (a=>new AuthorDto { 
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Biography = a.Biography,
            DateOfBirth = a.DateOfBirth
            }).ToList();
        }
        public async Task<AuthorDto> GetAuthorByIdAsync(int id,CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, cancellationToken);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found");

            return new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Biography = author.Biography,
                DateOfBirth = author.DateOfBirth
            };
        }

        public async Task<int> CreateAuthorAsync(CreateAuthorDto authorDto,CancellationToken cancellationToken)
        {
            // DTO -> entity
            var author = new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                Biography = authorDto.Biography,
                DateOfBirth = authorDto.DateOfBirth
            };

            await _unitOfWork.Authors.AddAsync(author,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return author.Id;
        }

        public async Task UpdateAuthorAsync(int id, UpdateAuthorDto authorDto,CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, cancellationToken);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found");

            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            author.Biography = authorDto.Biography;
            author.DateOfBirth = authorDto.DateOfBirth;

            await _unitOfWork.Authors.UpdateAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, cancellationToken);
            if (author == null)
                throw new NotFoundException($"Author with ID {id} not found");

            await _unitOfWork.Authors.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<BookDto>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken)
        {
            //BookRepositorys gamovikeneb wignebis sapovnelad
            //anu akedanve gadavwvdet unda da raki UnitOfWork makvs pirdapir makedan  mivwvd

            //IBook reposhi momiwevs metodis damateba an memoryshi gavfiltravdi tu didi aplikacia araa 
            //swori gza memgoni reposmeshveobit ikneboda ,
            var allBooks = await _unitOfWork.Books.GetAllAsync(new Domain.Parameters.BookParameters { PageSize = 1000 },cancellationToken);
            var authorBooks = allBooks.Where(b => b.AuthorId == authorId).ToList();
            return authorBooks.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                PublicationYear = b.PublicationYear,
                Description = b.Description,
                Quantity = b.Quantity,
                IsAvailable = b.Quantity > 0
            }).ToList();

        }
    }
}
