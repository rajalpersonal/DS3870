using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace getVehicles
{
    public static class getVehicles
    {
        private class Brand
        {
            public string Name { get; set; }
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public Brand(string strName, string strStreetAddress, string strCity, string strState, string strZip)
            {
                Name = strName;
                StreetAddress = strStreetAddress;
                City = strCity;
                State = strState;
                Zip = strZip;
            }
        }
        private class Vehicle
        {
            public Brand Brand { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public double MPG { get; set; }
            public Vehicle(Brand objBrand, string strModel, int intYear, double dblMPG)
            {
                Brand = objBrand;
                Model = strModel;
                Year = intYear;
                MPG = dblMPG;
            }
        }

        [FunctionName("getVehicles")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string strBrand = req.Query["Brand"];
            string strModel = req.Query["Model"];

            log.LogInformation("HTTP trigger on getVehicles processed a request for: " + strBrand);

            Brand Toyota = new Brand("Toyota", "1540 Interstate Dr", "Cookeville", "TN", "38501");
            Brand Ford = new Brand("Ford", "1600 Interstate Dr", "Cookeville", "TN", "38501");
            Brand Volkswagen = new Brand("Volkswagen", "2431 Gallatin Pike", "Madison", "TN", "37115");

            Vehicle Camry = new Vehicle(Toyota, "Camry", 2022, 28);
            Vehicle Supra = new Vehicle(Toyota, "Supra", 2022, 25);
            Vehicle Mustang = new Vehicle(Ford, "Mustang", 2021, 21);
            Vehicle Bronco = new Vehicle(Ford, "Bronco", 2021, 20);
            Vehicle Jetta = new Vehicle(Volkswagen, "Jetta", 2021, 30);
            Vehicle Golf = new Vehicle(Volkswagen, "Golf", 2021, 29);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            List<Vehicle> arrVehicle = new List<Vehicle>();
            arrVehicle.Add(Camry);
            arrVehicle.Add(Supra);
            arrVehicle.Add(Mustang);
            arrVehicle.Add(Bronco);
            arrVehicle.Add(Jetta);
            arrVehicle.Add(Golf);

            /*foreach(Vehicle vCurrent in arrVehicle)
            {
                if(vCurrent.Brand == Toyota)
                {
                    lstToyota.Add(vCurrent);
                }
                else if (vCurrent.Brand == Ford)
                {
                    lstFord.Add(vCurrent);
                }
                else
                {
                    lstVolkswagen.Add(vCurrent);
                }
            }*/

            List<Vehicle> lstVehicle = new List<Vehicle>();
            foreach (Vehicle vCurrent in arrVehicle)
            {
                if (strModel == vCurrent.Model && strBrand == vCurrent.Brand.Name)
                {
                    lstVehicle.Add(vCurrent);
                }
            }
            if (lstVehicle.Count > 0)
            {
                return new OkObjectResult(lstVehicle);
            }
            else
            {
                return new OkObjectResult("Car Not Found");
            }
        }
    }
}
