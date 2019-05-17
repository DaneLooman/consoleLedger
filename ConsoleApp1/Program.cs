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
        //Start Menu (create account, log in and run main menu, or shut down program)
       static Tuple<bool, List<User>, List<Transaction>> Start(List<User> users, List<Transaction> transactions)
        {
            User currentUser = new User();
            Console.Clear();
            Console.WriteLine("Chose an option:\n1: Create New Account\n2: Log In\n3: Shut Down");
            int selection;
            bool success = Int32.TryParse(Console.ReadLine(), out selection);
            if (success)
            {
                if (selection == 1)
                {
                    Console.WriteLine("Please enter your full email address");
                    string emailResult = Console.ReadLine().Trim().ToLower();

                    if (users.FindIndex(u => u.UserAcctName == emailResult) < 0)
                    {
                        currentUser.UserAcctName = emailResult;
                        Console.WriteLine("Please create a password");
                        string passResult1 = Console.ReadLine();

                        Console.WriteLine("Confirm your password");
                        string passResult2 = Console.ReadLine();

                        if (passResult1 == passResult2)
                        {
                        }
                        else
                        {
                            while (passResult1 != passResult2)
                            {
                                Console.WriteLine("Sorry. Your passwords did not match. Please try again.");
                                Console.WriteLine("Please create a password");
                                passResult1 = Console.ReadLine();

                                Console.WriteLine("Confirm your password");
                                passResult2 = Console.ReadLine();
                            }
                        }
                        currentUser.UserAcctPassword = passResult1;
                        currentUser.UserAcctId = users.Count + 1;
                        users.Add(currentUser);
                        return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);

                    }
                    else
                    {
                        Console.WriteLine("This user already exists. Press enter to restart.");
                        Console.ReadLine();
                        return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                    }

                }
                else if (selection == 2)
                {
                    bool loggedIn = false;
                    Console.Clear();
                    Console.WriteLine("Type your email address to log in.");
                    string result = Console.ReadLine().ToLower();
                    if (users.FindIndex(u => u.UserAcctName == result) >= 0)
                    {
                        int attempts = 0;
                        User attemptUser = users.Find(u => u.UserAcctName == result);
                        string attemptPass;
                        Console.WriteLine("Please enter your password");
                        attemptPass = Console.ReadLine();

                        while (attemptPass != attemptUser.UserAcctPassword && attempts < 3)
                        {
                            Console.WriteLine("Sorry. That password does not match what we have on file. Please enter your password again.");
                            attemptPass = Console.ReadLine();
                            attempts++;
                        }

                        if (attemptPass != attemptUser.UserAcctPassword && attempts >= 3)
                        {
                            Console.WriteLine("Sorry. Maximum attempts (3) exceeded.");
                        }
                        else
                        {
                            currentUser = attemptUser;
                            loggedIn = true;
                        }
                        while (loggedIn == true)
                        {
                            var results = (MainMenu(currentUser, transactions));
                            transactions = results.Item2;
                            loggedIn = results.Item1;
                        }
                        Console.WriteLine("You have logged out. Press Enter to return to the Main Menu.");
                        Console.ReadLine();
                        return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                    }
                    else
                    {
                        Console.WriteLine("Sorry. We could not find a user with that email address. Press Enter to try again.");
                        Console.ReadLine();
                        return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                    }
                }
                else if (selection == 3)
                {
                    Console.WriteLine("Are you sure you want to shut down? Y/N?");
                    string result = Console.ReadLine().ToLower();
                    if (result == "y")
                    {
                        return new Tuple<bool, List<User>, List<Transaction>>(false, users, transactions);
                    } else
                    {
                        Console.WriteLine("Shut Down Cancelled. Press Enter to restart");
                        Console.ReadLine();
                        return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                    }

                }
                else {
                    Console.WriteLine("Please only enter numbers 1, 2, or 3. Press Enter to restart");
                    Console.ReadLine();
                    return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                }

            }
            else
            {
                Console.WriteLine("Please only enter numbers 1, 2, or 3. Press Enter to restart");
                Console.ReadLine();
            }
            return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
        }
                    
        //Main Menu method once a User is logged in.
        static Tuple<bool, List<Transaction>> MainMenu(User user, List<Transaction> _transactions)
        {
            {
                List<Transaction> transactions = _transactions;
                Console.Clear();
                Console.WriteLine(
                   user.UserAcctName + " Logged In\n Options:  \n 1: Deposit\n 2: Withdraw\n 3: Balance Check\n 4: History\n 5: Logout");
                int selection;
                bool success = Int32.TryParse(Console.ReadLine(),out selection);
                if (success)
                {
                    if (selection == 1)
                    {
                        Transaction transaction = Deposit(user.UserAcctId);
                        if (transaction != null)
                        {
                            Console.WriteLine("Are you sure you want to deposit $" + transaction.Amt + "? Y/N");
                            string input = Console.ReadLine().ToLower();
                            if (input == "y")
                            {
                                transaction.Id = transactions.Count + 1;
                                transactions.Add(transaction);
                                Console.WriteLine("Deposit successful. Press enter.");
                                Console.ReadLine();
                                return new Tuple<bool, List<Transaction>>(true, transactions);
                            }
                            else
                            {
                                Console.WriteLine("Deposit cancelled. Press enter.");
                                Console.ReadLine();
                                return new Tuple<bool, List<Transaction>>(true, transactions);
                            }
                        } else
                        {
                            return new Tuple<bool, List<Transaction>>(true, transactions);
                        }

                    }
                    else if (selection == 2)
                    {   
                        Transaction transaction = Withdrawl(user.UserAcctId);
                        if (transaction != null)
                        {
                            Console.WriteLine("Are you sure you want to withdrawl $" + (transaction.Amt * -1) + "? Y/N");
                            string input = Console.ReadLine().ToLower();
                            if (input == "y")
                            {
                                if ((Balance(user.UserAcctId, transactions) + transaction.Amt) > 0)
                                {
                                    transaction.Id = transactions.Count + 1;
                                    transactions.Add(transaction);
                                    Console.WriteLine("Withdrawl successful. Press enter.");
                                    Console.ReadLine();
                                    return new Tuple<bool, List<Transaction>>(true, transactions);
                                }
                                else
                                {
                                    Console.WriteLine("Withdrawl cancelled. Balance too low. Press enter.");
                                    Console.ReadLine();
                                    return new Tuple<bool, List<Transaction>>(true, transactions);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Withdrawl cancelled. Press enter.");
                                Console.ReadLine();
                                return new Tuple<bool, List<Transaction>>(true, transactions);
                            }
                        } else
                        {
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
                        History(user.UserAcctId, transactions);
                        Console.WriteLine("This is the end of your history. Press enter.");
                        Console.ReadLine();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                    else if (selection == 5)
                    {
                        return new Tuple<bool, List<Transaction>>(false, transactions);
                    }
                    else
                    {
                        Console.WriteLine("Please only input numbers 1, 2, 3, 4, or 5.");
                        Console.ReadLine();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                } else
                {
                    Console.WriteLine("Please only input numbers. Press enter to try again.");
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
            decimal inputAmt;
            bool success = decimal.TryParse(Console.ReadLine(),out inputAmt);

            if (success)
            {
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
            else
            {
                Console.WriteLine("Please only enter values in 00.00 format with no symbols. Press enter to rety.");
                Console.ReadLine();
                return null;
            }          
        }
        
        //Withdrawl Transaction Creation
        static Transaction Withdrawl(int userId)
        {
            Console.WriteLine("How much would you like to withdrawl?");
            decimal inputAmt;
            bool success = Decimal.TryParse(Console.ReadLine(), out inputAmt);

            if (success)
            {
                Console.WriteLine("You may add a note to this transaction.");
                string inputMemo = Console.ReadLine();

                Transaction transaction = new Transaction
                {
                    Amt = inputAmt * -1,
                    Memo = inputMemo,
                    UserId = userId
                };
                return transaction;
            } else
            {
                Console.WriteLine("Please only enter values in 00.00 format with no symbols. Press enter to rety.");
                Console.ReadLine();
                return null;
            }
            
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
        
        //List all Transactions. 
        static void History(int userId, List<Transaction> transactions)
        {
            foreach(Transaction t in transactions)
            {
                Console.WriteLine("Trans ID:" + t.Id + " Amt:$" + t.Amt + " Memo:" + t.Memo);
            }
        }

    //----------------------Main Program---------------------------//
        static void Main(string[] args)
        {
            List<User> userAccts = new List<User>();
            List<Transaction> transactions = new List<Transaction>();
            var seedResult = SeedData(userAccts, transactions);
            userAccts = seedResult.Item1;
            transactions = seedResult.Item2;
            User currentUser = new User();
            bool running = true;

            while (running == true)
            {
                var results = Start(userAccts, transactions);
                userAccts = results.Item2;
                transactions = results.Item3;
                running = results.Item1;
            }

            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }
}
