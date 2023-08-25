using SimpleORM.Samples.Models;
using SimpleORM.SQLite;

namespace SimpleORM.Samples;

public class Program
{
    static void Main()
    {
        var simpleSqLiteOrm = new SimpleSQLiteORM(new SQLiteProvider("Data Source=sqlite.db;Version=3;"));

        var query = simpleSqLiteOrm.Persons
            .Where(t => t.Id > 1 && t.FirstName == "Jane");
        
        var sql = query.BuildSQl();
        var persons = query.Execute(simpleSqLiteOrm);
        
       
        Console.WriteLine("--------------------------------------------------+");
        Console.WriteLine($"SQL: {sql}");
        Console.WriteLine("+----+----------------------+---------------------+");
        Console.WriteLine("| ID |      First Name      |      Last Name      |");
        Console.WriteLine("+----+----------------------+---------------------+");

        foreach (var person in persons)
        {
            Console.WriteLine($"| {person.Id,-2} | {person.FirstName,-20} | {person.LastName,-19} |");
            Console.WriteLine("+----+----------------------+---------------------+");
        }
    }

    public class SimpleSQLiteORM : SimpleORM
    {
        public SimpleSQLiteORM(SQLiteProvider dataProvider) : base(dataProvider)
        {
            Persons = new List<Person>().AsQueryable();
        }

        public IQueryable<Person> Persons { get; }
    }
}