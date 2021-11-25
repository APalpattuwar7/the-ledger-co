using System;
using System.Linq;
using System.Reflection;
using TheLedgerCo.Models;
using Xunit;

namespace TheLedgerCo.Tests
{
    public class DataProcessorTests
    {
        [Fact]
        public void Loan_ProcessLoanForBorrower_ShouldAddNewLoanToTheList()
        {
            var processor = new DataProcessor();
            Data data = GetDataForNewLoan();

            processor.ProcessLoanForBorrower(data);
            Assert.Single(processor.loans);
        }

        [Fact]
        public void Payment_ProcessPayment_ShouldIncreaseRepaymentAmount()
        {
            var processor = new DataProcessor();
            Data data = GetDataForNewLoan();

            var paymentData = new Data()
            {
                BankName = "IDIDI",
                BorrowersName = "Dale",
                LumpSumAmount = 1000,
                EMINumber = 5
            };

            processor.ProcessLoanForBorrower(data);
            processor.ProcessPayment(paymentData);

            Assert.Equal(1000, processor.loans.SingleOrDefault().RepaymentAmount);
            Assert.Equal(5, processor.loans.SingleOrDefault().EMICompletedBeforeRepayment);
        }

        #region CalculateRemainingEMI Tests

        [Fact]
        public void Balance_CalculateRemainingEMIWithoutRepayment_ShouldReturnRemainingEMI()
        {
            var processor = new DataProcessor();
            Data data = GetDataForNewLoan();
            data.EMINumber = 3;
            processor.ProcessLoanForBorrower(data);
            int emi = 500;
            decimal totalAmountToRepay = 0;
            decimal totalAmountPaid = 0;

            Type type = typeof(DataProcessor);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(x => x.Name == "CalculateRemainingEMI" && x.IsPrivate)
            .First();

            var remainingEMIs = (int)method.Invoke(null, new object[] { data, processor.loans.SingleOrDefault(), totalAmountToRepay, totalAmountPaid, emi });
            Assert.Equal(9, remainingEMIs);
        }

        [Fact]
        public void Balance_CalculateRemainingEMIWithRepayment_ShouldReturnRemainingEMI()
        {
            var processor = new DataProcessor();
            Data data = GetDataForNewLoan();
            data.EMINumber = 3;
            processor.ProcessLoanForBorrower(data);
            var loan = processor.loans.SingleOrDefault();
            loan.RepaymentAmount = 1000;
            int emi = 500;
            decimal totalAmountToRepay = 5300;
            decimal totalAmountPaid = 2000;

            Type type = typeof(DataProcessor);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(x => x.Name == "CalculateRemainingEMI" && x.IsPrivate)
            .First();

            var remainingEMIs = (int)method.Invoke(null, new object[] { data, loan, totalAmountToRepay, totalAmountPaid, emi });
            Assert.Equal(7, remainingEMIs);
        } 

        #endregion

        #region CalculateTotalAmountPaid Tests

        [Fact]
        public void Balance_CalculateTotalAmountPaidWithoutRepayment_ShouldReturnTotalAmountPaid()
        {
            var processor = new DataProcessor();
            Data data = GetDataForNewLoan();
            data.EMINumber = 3;
            processor.ProcessLoanForBorrower(data);
            int emi = 500;

            Type type = typeof(DataProcessor);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(x => x.Name == "CalculateTotalAmountPaid" && x.IsPrivate)
            .First();

            var totalAmountPaid = (decimal)method.Invoke(null, new object[] { data, processor.loans.SingleOrDefault(), emi });
            Assert.Equal(1500, totalAmountPaid);
        }

        [Fact]
        public void Balance_CalculateTotalAmountPaidWithRepayment_ShouldReturnTotalAmountPaid()
        {
            Data data = GetDataForNewLoan();
            data.EMINumber = 7;

            var processor = new DataProcessor();
            processor.ProcessLoanForBorrower(data);
            var loan = processor.loans.SingleOrDefault();
            loan.RepaymentAmount = 1000;
            loan.EMICompletedBeforeRepayment = 5;
            int emi = 500;

            Type type = typeof(DataProcessor);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(x => x.Name == "CalculateTotalAmountPaid" && x.IsPrivate)
            .First();

            var totalAmountPaid = (decimal)method.Invoke(null, new object[] { data, loan, emi });
            Assert.Equal(4500, totalAmountPaid);
        }

        [Fact]
        public void Balance_CalculateTotalAmountPaidWithRepaymentAndEMILessThanPaymentEMI_ShouldReturnTotalAmountPaid()
        {
            Data data = GetDataForNewLoan();
            data.EMINumber = 3;

            var processor = new DataProcessor();
            processor.ProcessLoanForBorrower(data);
            var loan = processor.loans.SingleOrDefault();
            loan.RepaymentAmount = 1000;
            loan.EMICompletedBeforeRepayment = 5;
            int emi = 500;

            Type type = typeof(DataProcessor);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(x => x.Name == "CalculateTotalAmountPaid" && x.IsPrivate)
            .First();

            var totalAmountPaid = (decimal)method.Invoke(null, new object[] { data, loan, emi });
            Assert.Equal(1500, totalAmountPaid);
        } 

        #endregion

        private static Data GetDataForNewLoan()
        {
            return new Data()
            {
                Amount = 5000,
                BankName = "IDIDI",
                BorrowersName = "Dale",
                NumberOfYears = 1,
                RateOfInterest = 6
            };
        }
    }
}
