using ConsoleApp1.Models.UserModels;
using ConsoleApp1.Models.UserTransactionModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        //<------------Methods------------------------------------->
        //Main Menu method returns a bool for if the user is logged in and returns a new version of the transaction list
        static Tuple<bool, List<Transaction>> MainMenu(User user, List<Transaction> _transactions)
        {
            {
                List<Transaction> transactions = _transactions;
                Console.Clear();
                Console.WriteLine(
                   user.UserAcctName + " Logged In\n Options:  \n 1: Deposit\n 2: Withdraw\n 3: Balance Check\n 4: History\n 5: Logout");
                int selection = Convert.ToInt32(Console.ReadLine());
                if (selection == 1)
                {
                    Transaction transaction = Deposit(user.UserAcctId);
                    Console.WriteLine("Are you sure you want to Deposit $" + transaction.Amt + "? Y/N");
                    string input = Console.ReadLine();
                    if (input == "Y")
                    {
                        transaction.Id = transactions.Count + 1;
                        transactions.Add(transaction);
                        Console.WriteLine("Deposit Successful. Press Enter.");
                        Console.ReadLine();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                    else
                    {
                        Console.WriteLine("Deposit Cancelled. Press Enter.");
                        Console.ReadLine();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }                  
                }
                else if (selection == 2)
                {
                    Transaction transaction = Withdrawl(user.UserAcctId);
                    Console.WriteLine("Are you sure you want to Withdrawl $" + (transaction.Amt * -1) + "? Y/N");
                    string input = Console.ReadLine();
                    if (input == "Y")
                    { if ((Balance(user.UserAcctId, transactions) + transaction.Amt) > 0)
                        {
                            transaction.Id = transactions.Count + 1;
                            transactions.Add(transaction);
                            Console.WriteLine("Withdrawl Successful. Press Enter.");
                            Console.ReadLine();
                            return new Tuple<bool, List<Transaction>>(true, transactions);
                        } else
                        {
                            Console.WriteLine("Withdrawl Cancelled. Balance too low. Press Enter.");
                            Console.ReadLine();
                            return new Tuple<bool, List<Transaction>>(true, transactions);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Withdrawl Cancelled. Press Enter.");
                        Console.ReadLine();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                }
                else if (selection == 3)
                {
                    Console.WriteLine("Your balance is $" + Balance(user.UserAcctId, transactions) + ". Press Enter.");
                    Console.ReadLine();
                    return new Tuple<bool, List<Transaction>>(true, transactions);
                }
                else if (selection == 4)
                {
                    Console.WriteLine("This is your history. Press Enter.");
                    Console.ReadLine();
                    return new Tuple<bool, List<Transaction>>(true, transactions);
                }
                else if (selection == 5)
                {
                    return new Tuple<bool, List<Transaction>>(false, transactions);
                }
                else
                {
                    Console.WriteLine("Please only input numbers.");
                    Console.ReadLine();
                    return new Tuple<bool, List<Transaction>>(true, transactions);
                }
            }
        }
        //Seed Data Dummy User and Transactions. 
        static Tuple<List<User>, List<Transaction>> SeedData(List<User> _users, List<Transaction> _transactions)
        {
            List<User> users = _users;
            List<Transaction> transactions = _transactions;
            if (users.Count == 0 && transactions.Count == 0)
            {
                User demoUser = new User
                {
                    UserAcctId = 1,
                    UserAcctName = "test@test.com",
                    UserAcctPassword = "password"
                };
                users.Add(demoUser);
                Transaction demoDeposit = new Transaction
                {
                    Amt = 99.50m,
                    Id = 1,
                    Memo = "First Deposit",
                    UserId = 1
                };
                Transaction demoWithdrawl = new Transaction
                {
                    Amt = -15.25m,
                    Id = 2,
                    Memo = "First Withdrawl",
                    UserId = 1
                };
                transactions.Add(demoDeposit);
                transactions.Add(demoWithdrawl);
                return new Tuple<List<User>, List<Transaction>>(users, transactions);

            } else {
                return new Tuple<List<User>, List<Transaction>>(users, transactions);
            };
        }
        // Deposit Transaction Creation
        static Transaction Deposit(int userId)
        {
            Console.WriteLine("How much would you like to deposit?");
            decimal inputAmt = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Please add a note to this transaction.");
            string inputMemo = Console.ReadLine();

            Transaction transaction = new Transaction
            {
                Amt = inputAmt,
                Memo = inputMemo,
                UserId = userId
            };
            return transaction;
        }
        //Withdrawl Transaction Creation
        static Transaction Withdrawl(int userId)
        {
            Console.WriteLine("How much would you like to Withdrawl?");
            decimal inputAmt = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Please add a note to this transaction.");
            string inputMemo = Console.ReadLine();

            Transaction transaction = new Transaction
            {
                Amt = inputAmt * -1,
                Memo = inputMemo,
                UserId = userId
            };
            return transaction;
        }
        //Check Balance Method - Takes in User and totals all their transactions. 
        static decimal Balance(int userId, List<Transaction> transactions)
        {
            decimal total = 0;
            foreach(Transaction t in transactions)
            {
                if(t.UserId == userId)
                {
                    total = total + t.Amt;
                }
            }
            return total;
        }



        static void Main(string[] args)
        {
            List<User> userAccts = new List<User>();
            List<Transaction> transactions = new List<Transaction>();
            var seedResult = SeedData(userAccts, transactions);
            userAccts = seedResult.Item1;
            transactions = seedResult.Item2;
            User currentUser = new User();
            bool loggedIn = false; 

            //allows for logging in and account creation. Once completed flips loggedIn to true and sets current User
            while (loggedIn == false){
                Console.Clear();
                Console.WriteLine("Type your email address to log in or \"new\" if you are a new user");
                string result = Console.ReadLine().ToLower();

                if (result == "new"){
                    Console.WriteLine("Please enter your full Email Address");
                    string emailResult = Console.ReadLine().Trim().ToLower();

                    if (userAccts.FindIndex(u => u.UserAcctName == emailResult) < 0) {
                        currentUser.UserAcctName = emailResult;
                            Console.WriteLine("Please enter a password");
                            string passResult1 = Console.ReadLine();

                            Console.WriteLine("Please re-enter a password");
                            string passResult2 = Console.ReadLine();

                            if (passResult1 == passResult2) {
                            }
                            else {
                                while (passResult1 != passResult2) { 
                                    Console.WriteLine("Sorry. Your password did not match. Please try again.");
                                    Console.WriteLine("Please enter a password");
                                    passResult1 = Console.ReadLine();

                                    Console.WriteLine("Please re-enter a password");
                                    passResult2 = Console.ReadLine();
                                }
                            }
                            currentUser.UserAcctPassword = passResult1;
                            currentUser.UserAcctId = userAccts.Count + 1;
                            userAccts.Add(currentUser);
                           }
                        else {
                            Console.WriteLine("This user already exists.");
                        }
                }
                else if (userAccts.FindIndex(u => u.UserAcctName == result) >= 0){
                    int attempts = 0;
                    User attemptUser = userAccts.Find(u => u.UserAcctName == result);
                    string attemptPass;
                    Console.WriteLine("Please Enter Your Password");
                    attemptPass = Console.ReadLine();

                    while(attemptPass != attemptUser.UserAcctPassword && attempts < 3) {                        
                        Console.WriteLine("Sorry. That password does not match what we have on file. Please enter your password again.");
                        attemptPass = Console.ReadLine();
                        attempts++;
                    }

                    if (attemptPass != attemptUser.UserAcctPassword && attempts >= 3){
                        Console.WriteLine("Sorry. Maximum attempts (3) exceeded.");
                    } else {
                        currentUser = attemptUser;
                        loggedIn = true;
                    }
                }
                else {
                    Console.WriteLine("Sorry. We could not find a user with that email address. Please try again.");
                }          
            }
            while (loggedIn == true)
            {
                var results = (MainMenu(currentUser, transactions));
                transactions = results.Item2;
                loggedIn = results.Item1;
            }

            Console.WriteLine("You have logged out");
            Console.Read();
        }
    }
}
