using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace TheApp.Services
{
    public class JsonFileLoader : IJsonFileLoader
    {
        private IHostingEnvironment _hostingEnvironment;

        public JsonFileLoader(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
 
        public async Task<object> LoadAsync(string pathFragment)
        {
            var full = Path.Combine(_hostingEnvironment.WebRootPath, pathFragment);
            var stream = new FileStream(full, FileMode.Open);
            string fileContents;
            using (StreamReader reader = new StreamReader(stream))
            {
                fileContents = reader.ReadToEnd();
            }
            var result = JsonConvert.DeserializeObject(fileContents);
            return result;
        }
    }
}
