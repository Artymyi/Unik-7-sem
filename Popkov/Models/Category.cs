namespace PopkovFinance.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Category(string name)
        {
            Name = name;
        }
    }
}
