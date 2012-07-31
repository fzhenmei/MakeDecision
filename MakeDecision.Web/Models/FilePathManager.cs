using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MakeDecision.Web.Models
{
    public class FilePathManager
    {
        private readonly ICacheService cacheService;

        public FilePathManager()
        {
            cacheService = new InMemoryCache();
        }

        private XElement FilePathConfig
        {
            get
            {
                var configFilePath = HttpContext.Current.Server.MapPath("~/app_data/filepath.xml");

                return cacheService
                    .Get("filepathconfig",
                         configFilePath,
                         () => XElement.Load(configFilePath));
            }
        }

        public bool ShouldUploadFile(string categoryName)
        {
            return FilePathConfig.Elements()
                .Any(x =>
                         {
                             XAttribute xAttribute = x.Attribute("Name");
                             return xAttribute != null && xAttribute.ToString() == categoryName;
                         });
        }

        public string GetFilePath(string categoryName)
        {
            return FilePathConfig.Elements()
                .Where(x =>
                           {
                               XAttribute xAttribute = x.Attribute("name");
                               return xAttribute != null && xAttribute.ToString() == categoryName;
                           })
                .Select(x =>
                            {
                                XAttribute attribute = x.Attribute("path");
                                return attribute != null ? attribute.ToString() : string.Empty;
                            })
                .FirstOrDefault();
        }
    }
}