using ConsoleApp1.Models.UserTransactionModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ConsoleApp1;

namespace UnitTests
{
    [TestClass]
    public class BalanceTest
    {
        [TestMethod]
        public void BalanceCheckGetsOnlyUsersTransactions()
        {
            //Arrange
            List<Transaction> testTransactions = new List<Transaction>();
            Transaction transaction1 = new Transaction
            {
                Amt = 50.25m,
                Id = 1,
                UserId = 100
            };
            testTransactions.Add(transaction1);
            Transaction transaction2 = new Transaction
            {
                Amt = 10.50m,
                Id = 2,
                UserId = 99
            };
            testTransactions.Add(transaction2);
            decimal expected = 50.25m;

            //Act
            var actual = Program.Balance(100, testTransactions);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
