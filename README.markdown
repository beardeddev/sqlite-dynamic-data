##Simple query engine for SQLite 3

Query engine for sqlite 3. Idea inspired by SQLCE quering. Not need to object mapping and classes.
Uses simple quering and dynamic property creation from query resul tset schema based on System.Dynamic.

**Required:**

- .NET 4.0
- Visual Studio 2010 Express (C#) to build from sources
- System.Data.SQLite as exturnal dependency

###Using query engine:

**To run System.Data.SQLite on .NET 4.0 add to configuration following lines:**

``
<startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0"/>
 </startup>
``
 
**Inherit from base class passing configuration string name as parameter 
to contructor in base class as follwing:**

``
public class DemoDb : SQLite.Dynamic.Data.Database
{
        public DemoDb()
            : base("demo")
        {        
        }
}
``

**So configuration of app should be next:**

``
<?xml version="1.0" encoding="utf-8" ?><configuration><connectionStrings><add name="demo" connectionString="Data Source=:memory:;New=True;Pooling=True;Max Pool Size=1;Journal Mode=Off;Version=3;" /></connectionStrings><startup useLegacyV2RuntimeActivationPolicy="true"><supportedRuntime version="v4.0"/</startup></configuration>
``

**And enjoy with engine:**

``
DemoDb db = new DemoDb();
``

**Execute query against database**

``
db.Execute(@"CREATE TABLE dictionary(id INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(256), description TEXT);");
``

``
Console.WriteLine("Table created");
``

**Inserting data to database**

``
int affectedRows = db.Execute("INSERT INTO dictionary(title, description) VALUES(@0, @1)", "Java", @"Java is a programming language originally developed by James Gosling at Sun Microsystems.");
``

**Quering data from database**

``
var res = db.Query("SELECT * FROM dictionary");            
foreach (var r in res)
{
     Console.WriteLine("{0}\t{1}\t...", r.id, r.title);
}
``

``
var first = db.Query("SELECT * FROM dictionary WHERE Id = @0", 1);            
Console.WriteLine("{0}\t{1}\t...", first.id, first.title);
``