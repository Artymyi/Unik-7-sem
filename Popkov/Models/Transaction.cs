namespace PopkovFinance.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public bool IsIncome { get; set; }

        public Category Category { get; set; }
        public User User { get; set; }

        public void Validate()
        {
            if (Amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            if (Date > DateTime.Now)
                throw new ArgumentException("Date cannot be in the future");
        }
    }
}
