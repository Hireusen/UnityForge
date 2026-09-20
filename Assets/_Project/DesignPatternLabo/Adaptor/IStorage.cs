namespace Project.DesignPatternLabo
{
    public interface IStorage
    {
        bool Save(string key, string jsonData);
        string Load(string key);
    }
}
