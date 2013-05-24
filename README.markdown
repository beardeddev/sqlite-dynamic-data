##Simple query engine for SQLite 3

Query engine for sqlite. Idea inspired by SQLCE quering. Not need an object mapping and classes.
Uses simple quering and dynamic property creation from query resultset schema based using System.Dynamic.

**Required:**

- .NET 4.0
- Visual Studio 2010 Express (C#) to build from sources
- System.Data.SQLite as exturnal dependency

###Using query engine:

**To run System.Data.SQLite on .NET 4.0 add to configuration following lines:**

```csharp
<startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0"/>
 </startup>
```
 
**Inherit from base class passing configuration string name as parameter 
to contructor in base class as follwing:**

```csharp
public class DemoDb : SQLite.Dynamic.Data.Database
{
        public DemoDb()
            : base("demo")
        {        
        }
}
```

**Configuring the app:**

```csharp
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <connectionStrings>
    <add name="demo" connectionString="Data Source=:memory:;New=True;Pooling=True;Max Pool Size=1;Journal Mode=Off;Version=3;" />
    <add name="other" connectionString="Data Source=:memory:;New=True;Pooling=True;Max Pool Size=1;Journal Mode=Off;Version=3;" />
  </connectionStrings>
  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0"/>
  </startup>
</configuration>
```

**And enjoy with engine:**

```csharp
DemoDb db = new DemoDb();
```

**Execute query against database**

```csharp
db.Execute(@"CREATE TABLE links(id INTEGER PRIMARY KEY AUTOINCREMENT, 
             title VARCHAR(256), url TEXT);");
```

**Inserting data to database**

```csharp
int affectedRows = db.Execute("INSERT INTO links(title, url) VALUES(@0, @1)", 
                              "google.com", "http://google.com");
```

**Quering data from database**

```csharp
var res = db.Query("SELECT * FROM links");            
```

**Query with parameters**
```csharp
var res = db.QuerySingle("SELECT * FROM links WHERE id = @0", 1);
```

**Open another datbase in runtime and run query**
```csharp
var db = Database.Open("other");
var res db.QueryScalar("SELECT COUNT(id) FROM posts where status = @0", true)
```
