using Kaspersky.Database;
using Kaspersky.Database.Models;
using System.Threading.Tasks;

namespace Kaspersky.Api
{
    internal static class Mockdata
    {
        public static async Task InsertAsync( BookshelfContext db )
        {
            await db.Books.AddRangeAsync
            (
                new Book()
                {
                    Isbn = "978-5-8459-2015-7",
                    Title = "C# 4.0 : полное руководство",
                    Publisher = "Вильямс",

                    PagesTotal = 1056,
                    PublicationYear = 2018,

                    Authors = new[]
                    {
                        new Author()
                        {
                            Name = "Г.",
                            Surname = "Шилдт"
                        }
                    }
                },
                new Book()
                {
                    Isbn = "978-5-94074-537-2",
                    Title = "Параллельное пр-е на С++ в действии",

                    PagesTotal = 672,
                    PublicationYear = 2016,

                    Authors = new[]
                    {
                        new Author()
                        {
                            Name = "Энтони",
                            Surname = "Уильямс"
                        },
                        new Author()
                        {
                            Name = "А. А.",
                            Surname = "Слинкин"
                        }
                    }
                },
                new Book()
                {
                    Isbn = "978-5-8459-1984-7",
                    Title = "Исксств пргрммрвня. Т.1. Осн. алг.",

                    PagesTotal = 722,
                    PublicationYear = 1998,

                    Authors = new[]
                    {
                        new Author()
                        {
                            Name = "Дональд",
                            Surname = "Кнут"
                        }
                    }
                },
                new Book()
                {
                    Isbn = "5-94157-229-8",
                    Title = "Техника отладки пргрмм без ис. т.",

                    PagesTotal = 823,

                    Authors = new[]
                    {
                        new Author()
                        {
                            Name = "Крис",
                            Surname = "Касперски"
                        }
                    }
                }
            );

            await db.SaveChangesAsync();
        }
    }
}
