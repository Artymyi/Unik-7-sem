namespace PopkovFinance.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<Goal> Goals { get; set; } = new List<Goal>();
    }
}
