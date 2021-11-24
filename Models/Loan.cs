using System;

namespace TheLedgerCo.Models
{
    public class Loan
    {
        public Bank Bank { get; set; }

        public Borrower Borrower { get; set; }

        public decimal PrincipalAmount { get; set; }

        public int NumberOfYears { get; set; }

        public int RateOfInterest { get; set; }

        public decimal RepaymentAmount { get; set; }

        public int EMICompletedBeforeRepayment { get; set; }


        public decimal CalculateTotalAmountToRepay()
        {
            var totalInterest = this.PrincipalAmount * this.NumberOfYears * this.RateOfInterest / 100;
            return totalInterest + this.PrincipalAmount;
        }

        public int CalculateEMI(decimal totalAmountToRepay)
        {
            return (int)Math.Ceiling(totalAmountToRepay / (this.NumberOfYears * 12));
        }
    }
}
