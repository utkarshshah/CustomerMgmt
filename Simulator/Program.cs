using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CustomerMgmtWebAPI.Models;
using Newtonsoft.Json;

namespace Simulator
{
    class Program
    {
        public static string apiBaseUrl = "http://localhost/CustomerMgmtWebAPI/Customer";
        static async Task Main(string[] args)
        {
            await SendPostRequests();
            await Task.Delay(1000); // Delay to ensure POST requests complete before GET requests
            Console.ReadLine();
            await SendGetRequest();
            Console.ReadLine();
        }

        static async Task SendPostRequests()
        {
            var random = new Random();
            var firstNames = new string[] { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
            var lastNames = new string[] { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

            var tasks = new List<Task>();
            for (int i = 0; i < 1; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var customers = new List<Customer>();
                    for (int j = 0; j < 2; j++)
                    {
                        var customer = new Customer
                        {
                            FirstName = firstNames[random.Next(0, firstNames.Length)],
                            LastName = lastNames[random.Next(0, lastNames.Length)],
                            Age = random.Next(18, 91),
                            Id = i * 2 + j + 1 // Sequential IDs
                        };
                        customers.Add(customer);
                    }

                    await PostCustomers(customers);
                }));
            }

            await Task.WhenAll(tasks);
        }

        static async Task PostCustomers(List<Customer> customers)
        {
            var apiUrl = $"{apiBaseUrl}/Add"; 
            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(customers);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, stringContent);

                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"POST success: {result}");
                }
                else
                {
                    Console.WriteLine($"POST failed: {result}");
                    Console.WriteLine($"POST failed Request : {jsonContent}");
                }
            }
        }

        static async Task SendGetRequest()
        {
            var apiUrl = $"{apiBaseUrl}/Get";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GET success:\n{result}");
                }
                else
                {
                    Console.WriteLine($"GET failed: {response.ReasonPhrase}");
                }
            }
        }
    }
}
