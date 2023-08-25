using SimpleORM.Samples.Models;

namespace SimpleORM.Samples.Helpers;

public static class DataSeed
{
    public static IQueryable<Person> GetPersons()
    {
        return new List<Person>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith" },
            new() { Id = 3, FirstName = "Michael", LastName = "Johnson" },
            new() { Id = 4, FirstName = "Emily", LastName = "Williams" },
        }.AsQueryable();
    }
}