/*
 * Copyright (c) 2010, Vitaliy Litvinenko
 * All rights reserved.
 * Find me on http://beardeddev.pp.ua
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SSQE.Test
{
    public class Facts
    {
        public Facts()
        {
            TestDb db = new TestDb();

            var tables = db.Query(@"SELECT name 
                                    FROM sqlite_master 
                                    WHERE type = 'table'");


            if (tables.Count == 0)
            {
                db.Execute(@"CREATE TABLE dictionary(
                           id INTEGER PRIMARY KEY AUTOINCREMENT,
                           title VARCHAR(256),
                           description TEXT
                        )
                        ;");
            }
        }

        [Fact]
        public void Test_Should_Init_Database_Class()
        {
            var db = Database.Open("test");
            Assert.NotNull(db);
        }

        [Fact]
        public void Test_Should_Exucute_Query()
        {
            var db = new TestDb();

            Assert.NotNull(db);

            int rows = db.Execute(@"INSERT INTO dictionary(title, description) VALUES('CMS', 
                                                        'A content management system (CMS) is the collection of procedures used to manage work 
                                                         flow in a collaborative environment. These procedures can be manual or computer-based.')");
            Assert.Equal(1, rows);
        }

        [Fact]
        public void Test_Should_Exucute_With_Parameters()
        {
            var db = new TestDb();

            Assert.NotNull(db);

            int rows = db.Execute(@"INSERT INTO dictionary(title, description) VALUES(@0, 
                                                        @1)", "CMS", @"A content management system (CMS) is the collection of procedures used to manage work 
                                                         flow in a collaborative environment. These procedures can be manual or computer-based.");
            Assert.Equal(1, rows);
        }

        [Fact]
        public void Test_Should_Query_For_Count()
        {
            var db = new TestDb();

            Assert.NotNull(db);

            int rows = db.Execute(@"INSERT INTO dictionary(title, description) VALUES(@0, 
                                                        @1)", "Textile", @"Textile is a lightweight markup language originally developed 
                                                            by Dean Allen and billed as a ""humane Web text generator"". 
                                                            Textile converts its marked-up text input to valid, well-formed XHTML and also  
                                                            inserts character entity references for apostrophes, opening and closing 
                                                            single and double quotation marks, ellipses and em dashes.");

            var res = db.Query(@"SELECT COUNT(*) as RecordsCount 
                                    FROM dictionary");

            Assert.NotNull(res);
            Assert.NotEqual(0, res.Count);
            Assert.NotEqual(0, res[0].RecordsCount);
        }

        [Fact]
        public void Test_Should_Query_All_Records()
        {
            var db = new TestDb();

            Assert.NotNull(db);

            var res = db.Query("SELECT * FROM dictionary");

            Assert.NotNull(res);
            Assert.NotEqual(0, res.Count);

            var count = db.Query(@"SELECT COUNT(*) as RecordsCount 
                                    FROM dictionary");

            Assert.NotNull(count);
            Assert.NotEqual(0, count.Count);
            Assert.Equal(res.Count, count[0].RecordsCount);
        }

        [Fact]
        public void Test_Should_Execute_Parametrized_Query()
        {
            var db = new TestDb();

            Assert.NotNull(db);

            int rows = db.Execute(@"INSERT INTO dictionary(title, description) VALUES(@0, 
                                                        @1)", "BBCode", @"Bulletin Board Code or BBCode is a 
                                                            lightweight markup language used to format posts in many message boards. 
                                                            The available tags are usually indicated by square brackets surrounding a keyword, 
                                                            and they are parsed by the message board system before being translated into a markup 
                                                            language that web browsers understand—usually HTML or XHTML.");

            Assert.Equal(1, rows);

            var res = db.Query("SELECT * FROM dictionary WHERE title = @0", "BBCode");

            Assert.NotNull(res);
            Assert.Equal(1, res.Count);

            Assert.Equal("BBCode", res[0].title);
            Assert.Same(typeof(System.Int64), res[0].id.GetType());
        }
    }
}
