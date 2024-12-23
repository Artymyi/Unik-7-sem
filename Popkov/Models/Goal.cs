namespace PopkovFinance.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime DueDate { get; set; } 

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Goal name cannot be empty");
            if (TargetAmount <= 0)
                throw new ArgumentException("Target amount must be greater than 0");
            if (DueDate < DateTime.Now)
                throw new ArgumentException("Due date must be in the future");
        }
    }
}
