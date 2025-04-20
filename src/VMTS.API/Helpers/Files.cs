namespace VMTS.API.Helpers;

public static class Files
{
    public static string UploadFile(IFormFile file, string folderName)
    {
        // 1. Get Located Folder Path
        //string folderPath = "D:\\Route\\Cycle 40\\07 ASP.NET Core MVC\\Session 05\\Demos\\G02 De
        //string folderPath = Directory. GetCurrentDirectory() +@"\wwwroot\files\"+ folderName;
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
        
        // 2. Get File Name and Make it UNIQUE
        string fileName = $"{Guid.NewGuid()}{file.FileName}";

        // 3. Get File Path
        string filePath = Path.Combine(folderPath, fileName);

        // 4. Save File as Streams[Data Per Time]

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        return fileName;//To Store in database
    }
    public static void DeleteFile(string fileName, string folderName)
    {

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}