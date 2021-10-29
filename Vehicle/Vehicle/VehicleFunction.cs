using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Vehicle
{
    public static class VehicleFunction
    {
        private class Car
        {
            public string Make { get; set; }
            public string Model { }
        }
        [FunctionName("VehicleFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string Make = req.Query["make"];
            string Model = req.Query["model"];
            string Year = req.Query["year"];
            bool Armored = req.Query["armored"];
            string Vin = req.Query["VIN"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            string strQuery = "INSERT INTO tblVehicles (Make, Model, Year, Armored, VIN) VALUES (@parMake, @parModel, @parYear, @parArmored, @parVIN)";
            DataSet dsSpyTable = new DataSet();
            string strConnection = "Server Provided";
            try
            {
                using (SqlConnection conSpyTable = new SqlConnection(strConnection))
                using (SqlCommand comSpyTable = new SqlCommand(strQuery, conSpyTable))
                {
                    SqlParameter parMake = new SqlParameter("parMake", SqlDbType.VarChar);
                    parMake.Value = @parMake;
                    comSpyTable.Parameters.Add(parMake);

                    SqlParameter parModel = new SqlParameter("parModel", SqlDbType.VarChar);
                    parModel.Value = @parModel;
                    comSpyTable.Parameters.Add(parModel);

                    SqlParameter parYear = new SqlParameter("parYear", SqlDbType.Int);
                    parYear.Value = @parYear;
                    comSpyTable.Parameters.Add(parYear);

                    SqlParameter parArmored = new SqlParameter("parArmored", SqlDbType.Bool);
                    parArmored.Value = @parArmored;
                    comSpyTable.Parameters.Add(parArmored);

                    SqlParameter parVIN = new SqlParameter("parVIN", SqlDbType.VarChar);
                    parVIN.Value = @parVIN;
                    comSpyTable.Parameters.Add(parVIN);

                    comSpyTable.Connection = conSpyTable;
                    comSpyTable.CommandText = strQuery;
                    conSpyTable.Open();
                    comSpyTable.ExecuteNonQuery();
                    conSpyTable.Close();
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex.Message.ToString());
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
