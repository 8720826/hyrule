namespace Yes.Infrastructure.Helpers
{
    public class ZipHelper
    {
        public static void ExtractToDirectory(string zipPath, string targetDirectory, string themeName)
        {
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            var requiredFileName = "config.json";

            using (var zipFile = new ZipFile(zipPath))
            {
                bool fileFound = false;

                foreach (ZipEntry entry in zipFile)
                {
                    if (entry.IsDirectory)
                        continue;

                    string entryName = entry.Name.Trim('/');
                    if (string.IsNullOrEmpty(entryName))
                        continue;

                    string[] parts = entryName.Split('/');

                    // 文件名必须匹配
                    if (!parts[^1].Equals(requiredFileName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // 所有父级目录必须为 themeName
                    bool allParentsAreTopDir = parts.Take(parts.Length - 1)
                                                    .All(p => p.Equals(themeName, StringComparison.OrdinalIgnoreCase));

                    if (allParentsAreTopDir)
                    {
                        fileFound = true;
                        break;
                    }
                }

                if (!fileFound)
                {
                    throw new FileNotFoundException(
                        $"ZIP 文件中未找到配置文件: {requiredFileName}");
                }


                foreach (ZipEntry entry in zipFile)
                {
                    string originalPath = entry.Name.Trim('/');
                    if (string.IsNullOrEmpty(originalPath)) continue;

                    string[] parts = originalPath.Split('/');
                    int repeatCount = 0;

                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i] == themeName)
                        {
                            repeatCount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    var newPathParts = new List<string>();
                    newPathParts.Add(themeName);
                    newPathParts.AddRange(parts.Skip(repeatCount));

                    string newRelativePath = Path.Combine(newPathParts.ToArray());
                    string targetPath = Path.Combine(targetDirectory, newRelativePath);

                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    else
                    {
                        if (!IsSafeFile(entry.Name))
                        {
                            continue;
                        }
                        using (var stream = zipFile.GetInputStream(entry))
                        {
                            using (var fileStream = File.Create(targetPath))
                            {
                                stream.CopyTo(fileStream);
                            }
                        }
                    }
                }
            }



        }

        private static string SanitizeEntryPath(string entryPath, string extractRoot)
        {
            // 路径安全处理
            string fullPath = Path.GetFullPath(Path.Combine(extractRoot, entryPath));

            // 验证是否在解压根目录内（防止路径遍历）
            if (!fullPath.StartsWith(Path.GetFullPath(extractRoot)))
            {
                throw new SecurityException("非法路径访问!");
            }

            return fullPath;
        }

        private static bool IsSafeFile(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return new[] { ".liquid", ".js", ".css", ".html", ".md", ".ttf", ".svg", ".eot", ".woff", ".woff2", ".toml", ".xml" }.Contains(ext);
        }
    }
}
