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
            var prg = new Program();
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
            decimal expected2 = 10.50m;

            //Act
            var actual = prg.Balance(100, testTransactions);
            var actual2 = prg.Balance(99, testTransactions);

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected2, actual2);
        }
    }
}
