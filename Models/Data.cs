namespace TheLedgerCo.Models
{
    public class Data
    {
        public string Command { get; set; }

        public string BankName { get; set; }

        public string BorrowersName { get; set; }

        public decimal Amount { get; set; }

        public int NumberOfYears { get; set; }

        public int RateOfInterest { get; set; }

        public int EMINumber { get; set; }

        public int LumpSumAmount { get; set; }
    }
}
