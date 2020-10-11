using System;
using System.IO;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Infrastructure
{
    public static class FileUtils
    {

        public static async Task<string> GetQueryFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Queries", fileName)))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}