using Arasva.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Arasva.Core.Services.Implementation
{
    /// <summary>
    /// #URLContentRead
    /// </summary>
    public class ReadURLContentService : IReadURLContentService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public ReadURLContentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method is used to Read the content from upload file and post the reponse into the file #URLContentRead
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<GlobalResponse> ReadURLContent(string filepath, string path)
        {
            GlobalResponse apiResponse = new();
            var maxconcurrents = int.Parse(_configuration["URLReadMaxConcurrentRequests"].ToString());
            string outputFile = "";

            try
			{
                //File path related changes
                var urls = await File.ReadAllLinesAsync(filepath);
                var filename = Path.GetFileName(filepath);  
                outputFile = string.Format("{0}\\{1}\\Response_{2}", path, AppConstants.URLUploads, filename);

                //Do the Semaphore for 
                //Point 2: Limit the maximum number of concurrent requests to N
                var semaphore = new SemaphoreSlim(maxconcurrents);
                var tasks = new List<Task>();

                //Write the SteamWriter
                using var writer = new StreamWriter(outputFile, append: false);

                foreach (var url in urls)
                {
                    await semaphore.WaitAsync();   // Limit concurrency

                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            //Point 1: Use asynchronous I/O for all network request
                            var response = await _httpClient.GetAsync(url);
                            var content = await response.Content.ReadAsStringAsync();

                            lock (writer)
                            {
                                //Point 3: Write the result of each request to an output file along with the URL and the HTTP status code
                                writer.WriteLine("Request URL: " + url);
                                writer.WriteLine("Resonse Status: " + (int)response.StatusCode);
                                writer.WriteLine("Resonse Content:");
                                writer.WriteLine(content);
                                writer.WriteLine("==================== End ============");
                            }
                        }
                        catch (Exception ex)
                        {
                            //Point 4: Handle failures for individual URLs without stopping the entire processing
                            lock (writer)
                            {
                                writer.WriteLine("Request URL: " + url);
                                writer.WriteLine("Resonse Status: ERR, " + ex.Message);
                                writer.WriteLine("Resonse Content:");
                                writer.WriteLine("==================== End ============");
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }

                    }));
                }

                await Task.WhenAll(tasks);

                //return response 
                apiResponse.message = string.Format(AppConstants.ActionSuccess);
                apiResponse.success = true;
                apiResponse.data = "URL read content written to file: " + outputFile;
            }
            catch (Exception ex)
            {
                apiResponse.success = false;
                apiResponse.error = string.Format(AppConstants.ErrorMessage, ex.Message); 
            }
            return apiResponse;
        }
    }
}
