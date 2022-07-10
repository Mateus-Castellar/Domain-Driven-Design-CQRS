namespace DDD.Pagamentos.AntiCorruption
{
    public interface IConfigurationManager
    {
        //simula uma classe que obtem dados de configs..
        string GetValue(string node);
    }

    public class ConfigurationManager : IConfigurationManager
    {
        public string GetValue(string node)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(lbda => lbda[new Random().Next(lbda.Length)]).ToArray());
        }
    }
}