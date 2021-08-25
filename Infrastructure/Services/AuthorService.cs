using System;
using System.Linq;
using Database.Models;
using Database.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IDatabaseContext _context;

        public AuthorService(IDatabaseContext context) =>
            _context = context;

        public IEnumerable<AuthorEntity> GetAll() =>
            _context.Authors;

        public IEnumerable<AuthorEntity> GetAll(Func<AuthorEntity, bool> rule) =>
            GetAll().Where(rule);

        public AuthorEntity Get(int authorId) =>
            GetAll().Single(a => a.Id == authorId);

        public AuthorEntity Get(string firstName, string middleName, string lastName) =>
            GetAll().Single(a => a.FirstName == firstName &&
                                 a.MiddleName == middleName &&
                                 a.LastName == lastName);

        public void Insert(AuthorEntity author) =>
            _context.Authors.Add(author);

        public void Update(AuthorEntity author)
        {
            var oldAuthor = Get(author.Id);
            oldAuthor.FirstName = author.FirstName;
            oldAuthor.MiddleName = author.MiddleName;
            oldAuthor.LastName = author.LastName;
        }

        public void Delete(int authorId) =>
            _context.Authors.Remove(Get(authorId));

        public void Delete(string firstName, string middleName, string lastName) =>
            _context.Authors.Remove(Get(firstName, middleName, lastName));

        public bool Contains(int authorId) =>
            _context.Authors.Any(a => a.Id == authorId);

        public bool Contains(string firstName, string middleName, string lastName) =>
            _context.Authors.Any(a => a.FirstName == firstName &&
                                      a.MiddleName == middleName &&
                                      a.LastName == lastName);

        public void Save()
        {
            var entries = _context.ChangeTracker.Entries();
            foreach (var entry in entries.Where(e => e.State == EntityState.Added))
            {
                var entity = entry.Entity as Expansion;
                if (entity is null)
                    continue;
                entity.TimeCreate = DateTimeOffset.Now;
                entity.TimeEdit = entity.TimeCreate;
                entity.Version = 1;
                if (entry.Entity is LibraryCardExpansion)
                    (entry.Entity as LibraryCardExpansion).TimeReturn = entity.TimeCreate.AddDays(7);
            }
            foreach (var entry in entries.Where(e => e.State == EntityState.Modified))
            {
                var entity = entry.Entity as Expansion;
                if (entity is null)
                    continue;
                entity.TimeEdit = DateTimeOffset.Now;
                entity.Version++;
            }
            _context.SaveChanges();
        }
    }
}
