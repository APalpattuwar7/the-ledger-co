using System;
using TheLedgerCo.Models;
using static TheLedgerCo.Enums.Enums;

namespace TheLedgerCo
{
    class Program
    {
        private static DataProcessor processor;

        static void Main(string[] args)
        {
            ProcessInputFile(args[0]);
        }

        private static void ProcessInputFile(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            processor = new DataProcessor();
            foreach (string line in lines)
            {
                ProcessInputData(line);
            }
        }

        private static void ProcessInputData(string line)
        {
            string[] input = line.Split(' ');
            string command = input[0];
            
            if(command == Command.LOAN.ToString())
            {
                var data = new Data()
                {
                    BankName = input[1],
                    BorrowersName = input[2],
                    Amount = Convert.ToDecimal(input[3]),
                    NumberOfYears = Convert.ToInt32(input[4]),
                    RateOfInterest = Convert.ToInt32(input[5])
                };

                processor.ProcessLoanForBorrower(data);

            } else if(command == Command.BALANCE.ToString())
            {
                var data = new Data()
                {
                    BankName = input[1],
                    BorrowersName = input[2],
                    EMINumber = Convert.ToInt32(input[3])
                };

                processor.ProcessBalance(data);

            } else if(command == Command.PAYMENT.ToString())
            {
                var data = new Data()
                {
                    BankName = input[1],
                    BorrowersName = input[2],
                    LumpSumAmount = Convert.ToInt32(input[3]),
                    EMINumber = Convert.ToInt32(input[4])
                };

                processor.ProcessPayment(data);
            }
        }
    }
}
