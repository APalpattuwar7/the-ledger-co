using System;
using System.Collections.Generic;
using System.Linq;
using TheLedgerCo.Models;

namespace TheLedgerCo
{
    public class DataProcessor
    {
        public readonly List<Loan> loans;

        public DataProcessor()
        {
            loans = new List<Loan>();
        }

        public void ProcessLoanForBorrower(Data data)
        {
            var loan = new Loan()
            {
                Bank = new Bank { Name = data.BankName },
                Borrower = new Borrower { Name = data.BorrowersName },
                PrincipalAmount = data.Amount,
                NumberOfYears = data.NumberOfYears,
                RateOfInterest = data.RateOfInterest
            };

            loans.Add(loan);
        }

        public void ProcessPayment(Data data)
        {
            Loan loan = GetProcessedLoan(data);
            loan.RepaymentAmount += data.LumpSumAmount;
            loan.EMICompletedBeforeRepayment = data.EMINumber;
        }

        public void ProcessBalance(Data data)
        {
            Loan loan = GetProcessedLoan(data);
            if (loan == null) return;

            var totalAmountToRepay = loan.CalculateTotalAmountToRepay();
            var emi = loan.CalculateEMI(totalAmountToRepay);
            var totalAmountPaid = CalculateTotalAmountPaid(data, loan, emi);
            var remainingEMI = CalculateRemainingEMI(data, loan, totalAmountToRepay, totalAmountPaid, emi);

            Console.WriteLine($"{loan.Bank.Name} {loan.Borrower.Name} {totalAmountPaid} {remainingEMI}");
        }

        #region PRIVATE Methods

        private static int CalculateRemainingEMI(Data data, Loan loan, decimal totalAmountToRepay, decimal totalAmountPaid, int emi)
        {
            int remainingEMI;
            //If PAYMENT was not done, then remainingEMI calculation is simple and can be calculated using NumberOfYears the loan was taken
            // otherwise, remainingEMI can be calculated using remaining amount divided by emi.
            if (loan.RepaymentAmount == 0)
            {
                remainingEMI = (loan.NumberOfYears * 12) - data.EMINumber;
            }
            else
            {
                remainingEMI = (int)Math.Ceiling((totalAmountToRepay - totalAmountPaid) / emi);
            }

            return remainingEMI;
        }

        private static decimal CalculateTotalAmountPaid(Data data, Loan loan, int emi)
        {
            decimal totalAmountPaid;
            if (data.EMINumber < loan.EMICompletedBeforeRepayment)
            {
                totalAmountPaid = emi * data.EMINumber;
            }
            else
            {
                totalAmountPaid = emi * data.EMINumber + loan.RepaymentAmount;
            }

            return totalAmountPaid;
        }

        private Loan GetProcessedLoan(Data data)
        {
            //Find already processed loan based on bank name and borrower's name
            return loans.Where(loan => loan.Bank.Name == data.BankName && loan.Borrower.Name == data.BorrowersName).SingleOrDefault();
        } 

        #endregion
    }
}