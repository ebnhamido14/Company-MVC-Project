using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        //Upload
        public static string UploadFile(IFormFile File, string FolderName)
        {
            //1 Get Located Folder Path
            //Directory.GetCurrentDirectory()+"\\wwwroot\\Files\\Images"+ FolderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            //2 Get FileName And Make It Unique
            string FileName = $"{File.FileName}";

            //3 Get FilePath=[FolderPath+FileName] 
            string FilePath = Path.Combine(FolderPath, FileName);

            //4 Save File As Stream 
            using var filestream = new FileStream(FilePath, FileMode.Create);
            File.CopyTo(filestream);

            //5 Return FileName
            return FileName;



        }

        //Delete
        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
