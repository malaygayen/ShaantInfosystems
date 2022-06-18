using ShaantInfosystems.Web.Models;

namespace ShaantInfosystem.Web.Common
{
    public static class ImportCSV
    {
        public static List<NseModel> Import(string sourceFilePath, int dataType)
        {
            List<NseModel> listOfNseModel = new List<NseModel>();
            try
            {
                //Read the contents of CSV file.
                string csvData = File.ReadAllText(sourceFilePath);
                //Execute a loop over the rows.
                int i = 0;
                foreach (string row in csvData.Split('\n'))
                {
                    
                    if (!string.IsNullOrEmpty(row) && i != 0 )
                    {
                       
                        //Execute a loop over the columns.
                        string[] cellArray = row.Split(',');

                        listOfNseModel.Add(new NseModel()
                        {
                            DataType = dataType,
                            Date = Convert.ToDateTime(cellArray[0].Trim('"').Trim(' ')),
                            Open = Convert.ToDecimal(cellArray[1].Trim('"').Trim(' ')),
                            High = Convert.ToDecimal(cellArray[2].Trim('"').Trim(' ')),
                            Low = Convert.ToDecimal(cellArray[3].Trim('"').Trim(' ')),
                            Close = Convert.ToDecimal(cellArray[4].Trim('"').Trim(' ')),
                            SharesTraded = Convert.ToInt64(cellArray[5].Trim('"').Trim(' ')),
                            TurnOverInRsCore = Convert.ToDecimal(cellArray[6].Trim('"').Trim(' '))
                        });
                    }
                    i++;
                }
                
            }
            catch (Exception ex)
            {
                
            }
            return listOfNseModel;
        }
    }
}
