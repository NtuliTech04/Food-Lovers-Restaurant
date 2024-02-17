using System.IO;
using System.Web;

namespace FLRApplication.Utilities
{
    public class BinaryUpload
    {
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] fileBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            fileBytes = reader.ReadBytes((int)file.ContentLength);
            return fileBytes;
        }
    }
}