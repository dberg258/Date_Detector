using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CSHttpClientSample
{
    static class Program
    {
        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "891a8b1f9a7743548bb812274e375382";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. 
        const string uriBase =
            "https://eastus.api.cognitive.microsoft.com/vision/v2.0/ocr";

        static void Main()
        {
            // Get the path and filename to process from the user.
            Console.WriteLine("Optical Character Recognition:");
            Console.Write("Enter the path to an image with text you wish to read: ");
            string imageFilePath = Console.ReadLine();

            if (File.Exists(imageFilePath))
            {
                // Call the REST API method.
                Console.WriteLine("\nWait a moment for the results to appear.\n");
                MakeOCRRequest(imageFilePath).Wait();
            }
            else
            {
                Console.WriteLine("\nInvalid file path");
            }
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        static async Task MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(contentString);
               Console.WriteLine(json.ToString());

                List<int> dates = new List<int>();
                foreach (var val1 in json["regions"])
                {
                   foreach(var val2 in val1["lines"])
                    {
                        foreach (var val3 in val2["words"])
                        {
                            string text = val3["text"].ToString();
                            if(text.Length == 10 && int.TryParse(text.Substring(0,2), out int n) == true && int.TryParse(text.Substring(3, 2), out int k) == true
                                && int.TryParse(text.Substring(6), out int g) == true)
                            {
                                text = text.Substring(6) + text.Substring(0, 2) + text.Substring(3, 2);
                                dates.Add(Int32.Parse(text));
                            }
                            else if (text.Length == 8 && int.TryParse(text.Substring(0, 2), out int j) == true && int.TryParse(text.Substring(3, 2), out int p) == true
                            && int.TryParse(text.Substring(6), out int m) == true)
                            {
                                if(Int32.Parse(text.Substring(6,2)) < 30) {
                                    text = "20" + text.Substring(6) + text.Substring(0, 2) + text.Substring(3, 2);
                                }
                                else
                                {
                                    text = "19" + text.Substring(6) + text.Substring(0, 2) + text.Substring(3, 2);
                                }
                               // Console.WriteLine(text);
                                dates.Add(Int32.Parse(text));
                            }
                        }   
                    }
                }

                string currentDate = DateTime.UtcNow.Date.ToString("MM/dd/yyyy");
                currentDate = currentDate.Substring(6) + currentDate.Substring(0, 2) + currentDate.Substring(3, 2);

                if(dates.Max() < Int32.Parse(currentDate))
                {
                    Console.WriteLine("Invalid");
                }
                else
                {
                    Console.WriteLine("Valid");
                }
                foreach (var n in dates)
                {
                    Console.WriteLine(n);
                }
                Console.WriteLine("Current Date: " + Int32.Parse(currentDate));
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}