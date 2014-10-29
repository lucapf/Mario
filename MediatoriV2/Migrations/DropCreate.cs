using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace mediatori.Migrations
{
    public class DropCreate : System.Data.Entity.DropCreateDatabaseAlways<mediatori.Models.MainDbContext>
    {

        protected override void Seed(mediatori.Models.MainDbContext context)
        {
            // seed database here
            ClearDatabase(context );
        }




        private void ClearDatabase(mediatori.Models.MainDbContext context)
        {


            List<string> tables;

            tables = context.GetType().GetProperties().Where(x => x.PropertyType.Name == "DbSet`1").Select(x => x.Name).ToList();

            foreach (string table in tables)
            {
                Debug.WriteLine(table);
            }



        }
    }
}