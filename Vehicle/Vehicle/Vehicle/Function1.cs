using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Vehicle
{
    public static class Function1
    {
        [FunctionName("addVehicle")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("addVehicle called at " + System.DateTime.Now.ToLongDateString());

            string strMake = req.Query["strMake"];
            string strModel = req.Query["strModel"];
            int intYear = int.Parse(req.Query["intYear"]);
            string strArmored = req.Query["blnArmored"];
            bool blnArmored = false;
            if(strArmored == "true")
            {
                blnArmored = true;
            }
            string strVIN = req.Query["strVIN"];
            log.LogInformation("New Vehicle Passed - Make: " + strMake + " | Model: " + strModel + " | Year: " + intYear.ToString() + " | Armored: " + strArmored + " | VIN: " + strVIN);

            string strQuery = "INSERT INTO tblVehicles VALUES (@parMake, @parModel, @parYear, @parArmored, @parVIN)";
            using SqlConnection conSpy = new SqlConnection("MyConnectionString");
            using SqlCommand comSpy = new SqlCommand(strQuery, conSpy);
            {
                SqlParameter parMake = new SqlParameter("parMake", System.Data.SqlDbType.VarChar);
                parMake.Value = strMake;
                comSpy.Parameters.Add(parMake);

                SqlParameter parModel = new SqlParameter("parModel", System.Data.SqlDbType.VarChar);
                parModel.Value = strModel;
                comSpy.Parameters.Add(parModel);

                SqlParameter parYear = new SqlParameter("parYear", System.Data.SqlDbType.Int);
                parYear.Value = intYear;
                comSpy.Parameters.Add(parYear);

                SqlParameter parArmored = new SqlParameter("parArmored", System.Data.SqlDbType.Bit);
                parArmored.Value = blnArmored;
                comSpy.Parameters.Add(parArmored);

                SqlParameter parVIN = new SqlParameter("parVIN", System.Data.SqlDbType.VarChar);
                parVIN.Value = strVIN;
                comSpy.Parameters.Add(parVIN);

                comSpy.ExecuteNonQuery();
            }
            string strMessage = "Success";
            return new OkObjectResult(strMessage);
        }
    }
}
