using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Net.Sockets;
using System.Reflection.Metadata;

namespace EFCoreTrainingPart2
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("EFCore Learning Part 2!");

            //var dbContext = new SchoolContext();
            var context = new ApplicationDbContext();

            //ADDING BEHAVIORS

            //1 to 1
            ////Method 1 - principal entity uzerinden
            //Person person = new();
            //person.Name = "Farasat";
            //person.Address = new() { PersonAddress = "SarayMTK/Sumgayit" };

            //await context.AddAsync(person);
            //await context.SaveChangesAsync();

            ////Method 2 - dependent entity uzerinden
            //Address address = new() { PersonAddress = "Nizami/Baku", Person = new() { Name = "Mardan" } };

            //await context.AddAsync(address);
            //await context.SaveChangesAsync();

            //------------------------------
            //1 to many
            //1 to many de 1 to 1 kimidir data add etmekde
            //method 1
            //Blog blog = new() { Name = "farasatnovruzon.com" };
            //blog.Posts.Add(new Post { Title = "post1" });
            //blog.Posts.Add(new Post { Title = "post2" });
            //blog.Posts.Add(new Post { Title = "post3" });

            //Blog blog2 = new()
            //{
            //    Name = "A Blog",
            //    Posts = new HashSet<Post> { new(){Title="post 3"}
            //    }
            //};

            //await context.AddAsync(blog2);
            //await context.SaveChangesAsync();

            //method 2  - duzgun hesab edilmir, yeni dependantdan baslamayin, cunki yalniz bir data elave ede bilirsen

            //YENI, PRINCIPAL ENTITY NI SADECE OZU UCUN YARADA BILERSEN, AMMA DEPENDANT DA ISE MECBUREN PRINCIPALI DA YARATMALISAN

            //method 3 - dependant uzerinden principalin obyekti yaradilmadan, tekce foreign keyini vermekle
            //yalniz evvelceden var olan datalar uzerinden edilir
            //Post post = new()
            //{
            //    BlogId = 1,
            //    Title = "Post with foreign key"
            //};
            //await context.AddAsync(post);
            //await context.SaveChangesAsync();

            //--------------------------------------------
            //many to many

            //method 1 - n to n - default conventions
            //    public class Author
            //{
            //    public Author()
            //    {
            //        Books = new HashSet<Book>();
            //    }
            //    public int AuthorId { get; set; }
            //    public string AuthorName { get; set; }

            //    public ICollection<Book> Books { get; set; }
            //}

            //public class Book
            //{
            //    public Book()
            //    {
            //        Authors = new HashSet<Author>();
            //    }
            //    public int Id { get; set; }
            //    public string BookName { get; set; }
            //    public ICollection<Author> Authors { get; set; }
            //}

            //Book book = new()
            //{
            //    BookName = "A Book",
            //    Authors = new HashSet<Author>
            //    {
            //        new()
            //        {
            //            AuthorName = "Hilmi"
            //        },
            //        new()
            //        {
            //            AuthorName = "Fatma"
            //        },
            //    }
            //};

            //await context.Books.AddAsync(book);
            //await context.SaveChangesAsync();


            //method 2 - n to n - fluent api
            Author author = new()
            {
                AuthorName = "Mustafa",
                Books = new HashSet<BookAuthor>()
                {
                    new() {BookId = 1},
                    new() {Book = new() { BookName = "B Book" } },
                }
            };
            await context.AddAsync(author);
            await context.SaveChangesAsync();

        }

        public class Author
        {
            public Author()
            {
                Books = new HashSet<BookAuthor>();
            }
            public int AuthorId { get; set; }
            public string AuthorName { get; set; }

            public ICollection<BookAuthor> Books { get; set; }
        }

        public class Book
        {
            public Book()
            {
                Authors = new HashSet<BookAuthor>();
            }
            public int Id { get; set; }
            public string BookName { get; set; }
            public ICollection<BookAuthor> Authors { get; set; }
        }

        public class BookAuthor
        {
            public int BookId { get; set; }
            public int AuthorId { get; set; }
            public Book Book { get; set; }
            public Author Author { get; set; }
        }

        //public class Person
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public Address Address { get; set; }
        //}
        //public class Address
        //{
        //    public int Id { get; set; }
        //    public string PersonAddress { get; set; }
        //    public Person Person { get; set; }
        //}



        //public class Blog
        //{
        //    public Blog()
        //    {
        //        Posts = new HashSet<Post>();
        //    }//bu o demekdir ki, blogun obyektini yaradib ordan postlari cagiracagamsa, null vermesin hec vaxt, eminlik ucun
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public ICollection<Post> Posts { get; set; }
        //}

        //public class Post
        //{
        //    public int Id { get; set; }
        //    public string Title { get; set; }
        //    public int BlogId { get; set; }
        //    public Blog Blog { get; set; }
        //}



        public class ApplicationDbContext : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EfCorePart2;Trusted_Connection=True;");
            }

            //many to many default convention

            //many to many adding behaviours with fluent api
            public DbSet<Author> Authors { get; set; }
            public DbSet<Book> Books { get; set; }
            public DbSet<BookAuthor> BookAuthors { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configure the many-to-many relationship between Author and Book
                modelBuilder.Entity<BookAuthor>()
                    .HasKey(ba => new { ba.BookId, ba.AuthorId });

                modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Book)
                    .WithMany(b => b.Authors)
                    .HasForeignKey(ba => ba.BookId);

                modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(ba => ba.AuthorId);

                // Additional configurations if needed...

                base.OnModelCreating(modelBuilder);


                //public DbSet<Person> Persons;
                //public DbSet<Address> Addresses;

                //protected override void OnModelCreating(ModelBuilder modelBuilder)
                //{
                //    modelBuilder.Entity<Address>()
                //        .HasOne(a => a.Person)
                //        .WithOne(p => p.Address)
                //        .HasForeignKey<Address>(a => a.Id);
                //}

                //public DbSet<Blog> Blogs { get; set; }
                //public DbSet<Post> Posts { get; set; }
                //protected override void OnModelCreating(ModelBuilder modelBuilder)
                //{
                //    // Configure any additional model relationships or constraints here
                //    modelBuilder.Entity<Blog>()
                //        .HasMany(b => b.Posts)
                //        .WithOne(p => p.Blog)
                //        .HasForeignKey(p => p.BlogId);
                //}
            }




        }
    }
}
