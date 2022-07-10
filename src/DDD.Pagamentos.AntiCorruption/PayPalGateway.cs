namespace DDD.Pagamentos.AntiCorruption
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptionKey);
        string GetCardHashKey(string serviceKey, string cartaoCredito);
        bool CommitTransaction(string cardHaskKey, string orderId, decimal amount);
    }

    public class PayPalGateway : IPayPalGateway
    {
        public bool CommitTransaction(string cardHaskKey, string orderId, decimal amount)
        {
            //50% de chance para dar certo ou errado (pagamento)
            return new Random().Next(2) == 0;
        }

        public string GetCardHashKey(string serviceKey, string cartaoCredito)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(lbda => lbda[new Random().Next(lbda.Length)]).ToArray());
        }

        public string GetPayPalServiceKey(string apiKey, string encriptionKey)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(lbda => lbda[new Random().Next(lbda.Length)]).ToArray());
        }
    }
}