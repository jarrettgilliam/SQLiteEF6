namespace SQLiteTest;

using System.ComponentModel.DataAnnotations;

public class LocalCacheItem
{
    public string AssemblyQualifiedName { get; set; }
    
    [Key]
    public string CacheKey { get; set; }

    public DateTime LastModifiedDate { get; set; }

    [MaxLength]
    public string Data { get; set; }
}