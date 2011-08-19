/*
 * Copyright (c) 2010, Vitaliy Litvinenko
 * All rights reserved.
 * Find me on http://beardeddev.pp.ua
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SQLite.Dynamic.Data;

namespace SQLite.Dynamic.Data.Demo
{
    class Program
    {
        public class DemoDb : SQLite.Dynamic.Data.Database
        {
            public DemoDb()
                : base("demo")
            {
            }
        }

        static void Main(string[] args)
        {
            DemoDb db = new DemoDb();

            db.Execute(@"CREATE TABLE dictionary(
                           id INTEGER PRIMARY KEY AUTOINCREMENT,
                           title VARCHAR(256),
                           description TEXT
                        )
                        ;");

            Console.WriteLine("Table created");
            Console.WriteLine();

            int affectedRows = db.Execute("INSERT INTO dictionary(title, description) VALUES(@0, @1)", 
                                                           "Java", 
                                                           @"Java is a programming language originally developed by 
                                                           James Gosling at Sun Microsystems (which is now a subsidiary 
                                                           of Oracle Corporation) and released in 1995 as a core component 
                                                           of Sun Microsystems' Java platform. The language derives much of 
                                                           its syntax from C and C++ but has a simpler object model and fewer 
                                                           low-level facilities.");

            Console.WriteLine("Inserted {0} rows", affectedRows);

            affectedRows = db.Execute("INSERT INTO dictionary(title, description) VALUES(@0, @1)", "Ruby", @"A dynamic, open source programming language with a focus on simplicity and productivity.");

            Console.WriteLine("Inserted {0} rows", affectedRows);

            affectedRows = db.Execute("INSERT INTO dictionary(title, description) VALUES(@0, @1)", "C#", @"C# (pronounced 'see sharp') is a multi-paradigm programming language encompassing 
                                                imperative, declarative, functional, generic, object-oriented (class-based), 
                                                and component-oriented programming disciplines. 
                                                It was developed by Microsoft within the .NET initiative and later approved as a standard by 
                                                Ecma (ECMA-334) and ISO (ISO/IEC 23270). ");

            Console.WriteLine("Inserted {0} rows", affectedRows);
            Console.WriteLine();
            Console.WriteLine("Dictionary:");
            Console.WriteLine("Id\tTitle\tDescription");

            var res = db.Query("SELECT * FROM dictionary");
            
            foreach (var r in res)
            {
                Console.WriteLine("{0}\t{1}\t...", r.id, r.title);
            }

            var c = db.Query("SELECT COUNT(*) AS Count FROM dictionary");

            Console.WriteLine("Count of record is {0}", c[0].Count);

            Console.WriteLine();
            Console.WriteLine("Finding Ruby Programming language");
            Console.WriteLine("Id\tTitle\tDescription");

            var ruby = db.Query("SELECT * FROM dictionary WHERE title = @0", "Ruby");
            foreach (var r in ruby)
            {
                Console.WriteLine("{0}\t{1}\t...", r.id, r.title);
            }

            Console.ReadKey();
        }
    }
}
