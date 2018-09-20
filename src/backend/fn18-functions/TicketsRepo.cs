using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper;
using FN18.Core.Models;

namespace FN18.Functions
{
    public class TicketRepository
    {
        private string connectionString;
        public TicketRepository()
        {
            connectionString = Environment.GetEnvironmentVariable("Sql");
    }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        // public void Add(Product prod)
        // {
        //     using (IDbConnection dbConnection = Connection)
        //     {
        //         string sQuery = "INSERT INTO Product (Name, Quantity, Price)"
        //                         + " VALUES(@Name, @Quantity, @Price)";
        //         dbConnection.Open();
        //         dbConnection.Execute(sQuery, prod);
        //     }
        // }

        public IEnumerable<Ticket> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Ticket>("SELECT * FROM Tickets");
            }
        }

    }
}