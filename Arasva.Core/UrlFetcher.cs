namespace Arasva.Core
{
    public static class UrlFetcher
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task FetchUrlsAsync(string path, string inputFile, int maxConcurrency)
        {
            var urls = await File.ReadAllLinesAsync(inputFile);
            var semaphore = new SemaphoreSlim(maxConcurrency);
            var tasks = new List<Task>();

            string outputFile = string.Format("{0}_Output.txt", path, inputFile);

            using var writer = new StreamWriter(outputFile, append: false);

            foreach (var url in urls)
            {
                await semaphore.WaitAsync();   // Limit concurrency

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var response = await _httpClient.GetAsync(url);
                        var content = await response.Content.ReadAsStringAsync();

                        lock (writer)
                        {
                            writer.WriteLine("Request URL: " + url);
                            writer.WriteLine("Resonse Status: " + (int)response.StatusCode);
                            writer.WriteLine("Resonse Content:");
                            writer.WriteLine(content);
                            writer.WriteLine("==================== End ============");
                        }
                    }
                    catch (Exception ex)
                    {
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
        }
    }
}
