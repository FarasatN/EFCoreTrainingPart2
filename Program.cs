using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using static EFCoreTrainingPart2.Program;

namespace EFCoreTrainingPart2
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("EFCore Learning Part 2!");
            var context = new ApplicationDbContext();

            //1 to 1 - deleting -----------------------------------

            //Person? person = await context.Persons
            //    .Include(p => p.Address)
            //    //.FindAsync(1); - includedn sonra findi ist. ede bilmirik
            //    .FirstOrDefaultAsync(a => a.PersonId == 1);
            //if(person != null)
            //context.Addresses.Remove(person.Address);
            //await context.SaveChangesAsync();

            //Address? address = await context.Addresses
            //    .Include(a => a.Person)
            //    .FirstOrDefaultAsync(p => p.AddressId == 1002);
            //if(address != null)
            //    context.Persons.Remove(address.Person);
            //await context.SaveChangesAsync();

            //1 to n - deleting --------------------------------------
            //from Blog

            //Blog? blog = await context.Blogs
            //    .Include(b => b.Posts)
            //    .FirstOrDefaultAsync(b => b.BlogId == 1);

            ////foreach (var item in blog.Posts.ToList())
            ////{
            ////    Console.WriteLine(item.Content);
            ////}
            //Post? post = blog.Posts.FirstOrDefault(p => p.PostId == 3);
            //context.Posts.Remove(post);
            //await context.SaveChangesAsync();

            //from Post----xxxx
            //var posts = context.Blogs
            //    .Include(b => b.Posts)
            //    .Where(b=>b.BlogId == 1)
            //    .ToList();
            //foreach (var post in posts)
            //{
            //    Console.WriteLine(post);
            //}

            //n to n - deleting
            //Book? book = await context.Books
            //    .Include(b => b.BookAuthors)
            //    .FirstOrDefaultAsync(b => b.BookId == 1002);

            //var authors = context.Books
            //    .Where(b => b.BookId == 1002)
            //    .SelectMany(b => b.BookAuthors)
            //    .Select(ba => ba.Author)
            //    .ToList();

            //foreach (var author in authors)
            //{
            //    Console.WriteLine(author.Name);
            //    if (author.AuthorId == 2)
            //    {
            //        context.Authors.Remove(author);
            //        await context.SaveChangesAsync();
            //    }
            //}

            //Book? book = await context.Books
            //    .Include(ba => ba.BookAuthors)
            //    .FirstOrDefaultAsync(b => b.BookId == 1002);
            //context.BookAuthors.Remove();


            //Bu davranislar Fluent API ile konf. edilir:
            //Cascade delete - esas tabledan silinene datanin bagli table ile elaqeli silinmesini yaradir
            //Blog? blog = await context.Blogs.FindAsync(1);
            //context.Blogs.Remove(blog);
            //await context.SaveChangesAsync();

            //SetNull - cascade dan ferqli olaraq silerken yerine null qoyur
            //1 to 1 da setnull ola bilmez!
            //Blog? blog = await context.Blogs.FindAsync(2);
            //context.Blogs.Remove(blog);
            //await context.SaveChangesAsync();

            //Restrict - esas tabledan herhangi data silinmeye calisildiginda o dataya qarsiliq gelen dependant table'da elaqeli datalar varsa eger, bu silme islemini engelleyecekdir
            //silmeye imkan vermir

            //many to many elaqede yalniz cascade ola biler


            //==================================================
            //Backing Fields - table icinde columnlari, entity classlari icerisinde propertler ile degil, fieldlarla temsil eden bir ozelliktir
            //etmeyimizi saglayan bir ozelliktir
            //var person = await context.Persons.FindAsync(1002);
            //Person person2 = new()
            //{
            //    Name = "Person 101",
            //};
            //await context.Persons.AddAsync(person2);
            //await context.SaveChangesAsync();
            //Console.Read();

            //Backing Field Attributes in fluent api
            //var person3 = await context.Persons.FindAsync(1002);
            //Console.Read();

            //Field and Property Access - efcore default olaraq sorgulari property uzerinden edir

            //UsePropertyAccessMode - fluent api da conf. edilir:
            //.UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction) - bir nece nov ola biler
            //Field, FieldDuringConstruction, Property, PreferField, PreferFieldDuringConstruction, PreferProperty

            //Field-Only Properties - meselen, umumiyyetle bir propertyni gostermemek isteyirikse
            //fluent api de conf. edilir
            var person4 = await context.Persons.FindAsync(1002);
            Console.Read();


        }

        //1 to 1
        public class Person
        {
            public int PersonId { get; set; }
            public string name;
            //yuxaridaki fieldi backin filed kimi ya getter setter ile, ya da attribute ile teyin etmek olar
            //public string Name { get=>name.Substring(0,3); set=>name=value.Substring(0,3); }
            //[BackingField(nameof(name))]

            //Ya da Fluent API de conf. edilir
            //public string Name { get; set; }

            //Field-Only Properties
            public string GetName()
                => name;
            public string SetName(string value)
                => name = value;

            // Navigation property for one-to-one relationship
            public Address Address { get; set; }
        }
        public class Address
        {
            public int AddressId { get; set; }
            public string Street { get; set; }
            public string City { get; set; }

            // Navigation property for one-to-one relationship
            public Person Person { get; set; }
            public int PersonId { get; set; }
        }

        //1 to n
        public class Blog
        {
            public int BlogId { get; set; }
            public string Title { get; set; }

            // Navigation property for one-to-many relationship
            public ICollection<Post> Posts { get; set; }
        }
        public class Post
        {
            public int PostId { get; set; }
            public string Content { get; set; }

            // Foreign key property for one-to-many relationship
            public int BlogId { get; set; }

            // Navigation property for one-to-many relationship
            public Blog Blog { get; set; }
        }

        //n to n
        public class Book
        {
            public int BookId { get; set; }
            public string Title { get; set; }

            // Navigation property for the many-to-many relationship
            public ICollection<BookAuthor> BookAuthors { get; set; }
        }
        public class Author
        {
            public int AuthorId { get; set; }
            public string Name { get; set; }

            // Navigation property for the many-to-many relationship
            public ICollection<BookAuthor> BookAuthors { get; set; }
        }

        // Join entity for the many-to-many relationship
        public class BookAuthor
        {
            public int BookId { get; set; }
            public Book Book { get; set; }

            public int AuthorId { get; set; }
            public Author Author { get; set; }
        }

        public class ApplicationDbContext : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EfCorePart2;Trusted_Connection=True;");
            }

            public DbSet<Person> Persons { get; set; }
            public DbSet<Address> Addresses { get; set; }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<Book> Books { get; set; }
            public DbSet<Author> Authors { get; set; }
            public DbSet<BookAuthor> BookAuthors { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Person>()
                    //.Property(p => p.Name);
                    //Field-Only Properties ucun HasField olmamalidir ve asagidaki kimi olacaq
                    .Property(nameof(Person.name));
                //.HasField(nameof(Person.name));//backing field in fluent api

                modelBuilder.Entity<Person>()
                .HasOne(p => p.Address)
                .WithOne(a => a.Person)
                .HasForeignKey<Address>(a => a.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)   // One Blog has many Posts
                .WithOne(p => p.Blog)    // Each Post belongs to one Blog
                .HasForeignKey(p => p.BlogId)
                .OnDelete(DeleteBehavior.Cascade);



                modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });


                modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Book)
                    .WithMany(b => b.BookAuthors)
                    .HasForeignKey(ba => ba.BookId)
                    .OnDelete(DeleteBehavior.Cascade);


                modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Author)
                    .WithMany(a => a.BookAuthors)
                    .HasForeignKey(ba => ba.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

            }
        }





        //----------------------------------------------------
        //ADDING BEHAVIORS
        //var context = new ApplicationDbContext();

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

        //public class Author
        //{
        //    public Author()
        //    {
        //        Books = new HashSet<BookAuthor>();
        //    }
        //    public int AuthorId { get; set; }
        //    public string AuthorName { get; set; }

        //    public ICollection<BookAuthor> Books { get; set; }
        //}

        //public class Book
        //{
        //    public Book()
        //    {
        //        Authors = new HashSet<BookAuthor>();
        //    }
        //    public int Id { get; set; }
        //    public string BookName { get; set; }
        //    public ICollection<BookAuthor> Authors { get; set; }
        //}

        //public class BookAuthor
        //{
        //    public int BookId { get; set; }
        //    public int AuthorId { get; set; }
        //    public Book Book { get; set; }
        //    public Author Author { get; set; }
        //}

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



        //public class ApplicationDbContext : DbContext
        //{
        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    {
        //        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EfCorePart2;Trusted_Connection=True;");
        //    }

        //    //many to many default convention

        //    //many to many adding behaviours with fluent api
        //    public DbSet<Author> Authors { get; set; }
        //    public DbSet<Book> Books { get; set; }
        //    public DbSet<BookAuthor> BookAuthors { get; set; }

        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        // Configure the many-to-many relationship between Author and Book
        //        modelBuilder.Entity<BookAuthor>()
        //            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        //        modelBuilder.Entity<BookAuthor>()
        //            .HasOne(ba => ba.Book)
        //            .WithMany(b => b.Authors)
        //            .HasForeignKey(ba => ba.BookId);

        //        modelBuilder.Entity<BookAuthor>()
        //            .HasOne(ba => ba.Author)
        //            .WithMany(a => a.Books)
        //            .HasForeignKey(ba => ba.AuthorId);

        //        // Additional configurations if needed...

        //        base.OnModelCreating(modelBuilder);


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
        //}

        //-----------------------------------------------------------
        //1 to 1 - updating
        //Person person = new()
        //{
        //    PersonName = "Farasat",
        //    Address = new()
        //    {
        //        AddressName = "Baku, Nesimi"
        //    }
        //};

        //Person person2 = new()
        //{
        //    PersonName = "Mardan",

        //};

        //await context.AddAsync(person);
        //await context.AddAsync(person2);
        //await context.SaveChangesAsync();

        //1. hal - esas cedveldeki dataya bagli datani yenileme
        //Person? person = await context.Persons
        //    .Include(p => p.Address)
        //    .FirstOrDefaultAsync(p => p.PersonId == 1);
        ////Console.WriteLine(person.PersonName+"/"+person.Address.AddressName);

        //context.Addresses.Remove(person.Address);
        //person.Address = new()
        //{
        //    AddressName = "New Address"
        //};

        //await context.SaveChangesAsync();

        //2. hal - asili datanin elaqeli oldugu esas datani yenileme
        //Address? address = await context.Addresses.FindAsync(2);
        //address.AddressId = 1;
        //await context.SaveChangesAsync(); //-- bele xeta verecek, ancaq elaqeni kesmek lazimdir

        //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Addresses ON");

        //Address? address = await context.Addresses.FindAsync(5);
        //context.Addresses.Remove(address);
        //await context.SaveChangesAsync();//address silinse de in-memoryde hele de elimizdedir

        //Person? person = await context.Persons.FindAsync(1);
        //address.Person = person;
        //await context.Addresses.AddAsync(address);
        //await context.SaveChangesAsync();



        //one to many updating
        //1. esasdan asilini deyismek
        //Blog? blog = await context.Blogs
        //    .Include(b=>b.Posts)
        //    .FirstOrDefaultAsync(b=>b.BlogId==1);
        //Post? deletingPost = blog.Posts.FirstOrDefault(p => p.PostId == 2);
        //blog.Posts.Remove(deletingPost);
        //blog.Posts.Add(new() { PostTitle = "PostX"});
        //blog.Posts.Add(new() { PostTitle = "PostY"});
        //await context.SaveChangesAsync();

        //asilidan esasi deyismek
        //Post? post = await context.Posts.FindAsync(1003);
        //Console.WriteLine(post.PostTitle);
        //post.Blog = new()
        //{
        //    BlogName = "Test Blog"
        //};
        //await context.SaveChangesAsync();

        //Post? post = await context.Posts.FindAsync(2);
        //Blog? blog = await context.Blogs.FindAsync(1);
        //post.Blog = blog;
        //await context.SaveChangesAsync();

        //many to many updating

        //Book book1 = new() { Title = "1. Kitap" };
        //Book book2 = new() { Title = "2. Kitap" };
        //Book book3 = new() { Title = "3. Kitap" };
        //Author author1 = new() { Name = "1. Author" };
        //Author author2 = new() { Name = "2. Author" };
        //Author author3 = new() { Name = "3. Author" };
        //var bookAuthor1 = new BookAuthor
        //{
        //    Book = book1,
        //    Author = author1
        //};
        //var bookAuthor2 = new BookAuthor
        //{
        //    Book = book1,
        //    Author = author2
        //};
        //var bookAuthor3 = new BookAuthor
        //{
        //    Book = book2,
        //    Author = author1
        //};
        //var bookAuthor4 = new BookAuthor
        //{
        //    Book = book2,
        //    Author = author2
        //};
        //var bookAuthor5 = new BookAuthor
        //{
        //    Book = book2,
        //    Author = author3
        //};
        //var bookAuthor6 = new BookAuthor
        //{
        //    Book = book3,
        //    Author = author3
        //};
        //context.Add(bookAuthor1);
        //context.Add(bookAuthor2);
        //context.Add(bookAuthor3);
        //context.Add(bookAuthor4);
        //context.Add(bookAuthor5);
        //context.Add(bookAuthor6);
        //context.SaveChanges();

        //1. method
        //Book? book = await context.BookAuthors.FindAsync(1);//in default convention
        //Book? book = await context.Books
        //.Include(b => b.BookAuthors)
        //.ThenInclude(ba => ba.Author)
        //.FirstOrDefaultAsync(b => b.BookId == 1);
        //Console.WriteLine(book.Title);

        //Book? book = await context.Books.FindAsync(3);
        //Author? author = await context.Authors.FindAsync(1);
        //var bookAuthor = new BookAuthor
        //{
        //    Book = book,
        //    Author = author
        //};
        ////context.BookAuthors.Add(bookAuthor);
        //context.Add(bookAuthor);
        //await context.SaveChangesAsync();

        //2. method
        //Author? author = await context.Authors
        //    .Include(a => a.BookAuthors)
        //    //.FirstOrDefaultAsync(a => a.AuthorId == 3);
        ////Console.WriteLine(author?.Name);
        //foreach (var book in author.Books)
        //{
        //    if(book.BookId != 1)
        //    {
        //        author.BookAuthors.Remove(book);
        //    }
        //}
        //await context.SaveChangesAsync();

        //var author = context.Authors
        //.Include(a => a.BookAuthors)
        //.ThenInclude(ba => ba.Book)
        //.FirstOrDefault(a => a.AuthorId == 3);

        //foreach (var book in author.BookAuthors)
        //{
        //    if (book.BookId != 1)
        //    {
        //        author.BookAuthors.Remove(book);
        //    }
        //}
        //await context.SaveChangesAsync();



        //---------------------------------------------------
        //UPDATING BEHAVIOURS
        //1 to 1
        //public class Person
        //{
        //    public int PersonId { get; set; }
        //    public string PersonName { get; set; }

        //    // Navigation property for the one-to-one relationship
        //    public Address Address { get; set; }
        //}

        //public class Address
        //{
        //    public int AddressId { get; set; }
        //    public string AddressName { get; set; }

        //    public int PersonId { get; set; }

        //    // Navigation property for the one-to-one relationship
        //    public Person Person { get; set; }
        //}

        //1 to n
        //public class Blog
        //{
        //    public Blog()
        //    {
        //        Posts = new HashSet<Post>();
        //    }
        //    public int BlogId { get; set; }
        //    public string BlogName { get; set; }

        //    // Navigation property for the one-to-many relationship
        //    public ICollection<Post> Posts { get; set; }
        //}

        //public class Post
        //{
        //    public int PostId { get; set; }
        //    public string PostTitle { get; set; }

        //    // Foreign key property for the one-to-many relationship
        //    public int BlogId { get; set; }

        //    // Navigation property for the one-to-many relationship
        //    public Blog Blog { get; set; }
        //}

        //n to n


        //1 to 1


    }
}
