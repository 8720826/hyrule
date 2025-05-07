namespace Yes.Application.Installs
{
    public class FileService : IFileService
    {
        public void CopyFolder(string sources, string dest)
        {
            DirectoryInfo dinfo = new DirectoryInfo(sources);
            //注，这里面传的是路径，并不是文件，所以不能包含带后缀的文件                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                //目标路径destName = 新文件夹路径 + 源文件夹下的子文件(或文件夹)名字                
                //Path.Combine(string a ,string b) 为合并两个字符串                     
                string destName = Path.Combine(dest, f.Name);
                if (f is System.IO.FileInfo)
                {
                    //如果是文件就复制       
                    File.Copy(f.FullName, destName, true);//true代表可以覆盖同名文件                     
                }
                else
                {
                    //如果是文件夹就创建文件夹，然后递归复制              
                    Directory.CreateDirectory(destName);
                    CopyFolder(f.FullName, destName);
                }
            }
        }
    }
}
