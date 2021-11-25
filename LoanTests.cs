using TheLedgerCo.Models;
using Xunit;

namespace TheLedgerCo.Tests
{
    public class LoanTests
    {
        [Fact]
        public void Loan_CalculateTotalAmountToRepay_ReturnsTotalAmountToRepay()
        {
            var loan = new Loan();
            loan.PrincipalAmount = 5000;
            loan.NumberOfYears = 2;
            loan.RateOfInterest = 5;

            var totalAmountToRepay = loan.CalculateTotalAmountToRepay();
            Assert.Equal(5500, totalAmountToRepay);
        }

        [Fact]
        public void Loan_CalculateEMI_ReturnsEMI()
        {
            var loan = new Loan();
            loan.NumberOfYears = 2;

            var emi = loan.CalculateEMI(5500);
            Assert.Equal(230, emi);
        }
    }
}
