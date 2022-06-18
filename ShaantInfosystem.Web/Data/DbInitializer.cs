using ShaantInfosystem.Web.Common;

namespace ShaantInfosystem.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ShaantInfosystemWebContext context)
        {
            context.Database.EnsureCreated();
            if(!context.NseModel.Any())
            { 
                //import nifty 50 data
                var nifty50 = ImportCSV.Import("./CsvFiles/nifty50.csv", 1);
                foreach (var item in nifty50)
                { 
                    context.NseModel.Add(item);
                }
                context.SaveChanges();

                //import nifty 100 data
                var nifty100 = ImportCSV.Import("./CsvFiles/nifty100.csv", 2);
                foreach (var item in nifty100)
                {
                    context.NseModel.Add(item);
                }
                context.SaveChanges();

                //import nifty 200 data
                var nifty200 = ImportCSV.Import("./CsvFiles/nifty200.csv", 3);
                foreach (var item in nifty200)
                {
                    context.NseModel.Add(item);
                }
                context.SaveChanges();

                //import nifty 500 data
                var nifty500 = ImportCSV.Import("./CsvFiles/nifty500.csv", 4);
                foreach (var item in nifty500)
                {
                    context.NseModel.Add(item);
                }
                context.SaveChanges();
            }
        }
    }
}
