using System.Collections;
using System.IO;
using System.Linq;

namespace Frontend
{
    public class FileHandler
    {
        public void DownloadFile(byte[] saveData, string savePath, string fileName)
        {
            byte[] bytes = saveData;

            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);

            File.WriteAllBytes(tempFilePath, bytes);

            string destinationFilePath = savePath;
            
            if(Path.GetExtension(tempFilePath) != Path.GetExtension(destinationFilePath))
            {
                throw new Exception("invalid file extension");
            }

            File.Copy(tempFilePath, destinationFilePath, true);

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
            
        }
        public byte[] ReadFile(string savePath)
        {

            if (!Path.Exists(savePath)) return null;

            byte[] fileBytes = File.ReadAllBytes(savePath);
            return fileBytes;
        }
        public string ReadFileName(string savePath) //сомнительно нууу Okaaaay
        {
            if (!Path.Exists(savePath)) return null;

            //DirectoryInfo sourceDirInfo = new DirectoryInfo(savePath);
            //var fileName = sourceDirInfo.GetFiles().First().Name;

            var fileName = Path.GetFileName(savePath);
            return fileName;
        }
    }
}
