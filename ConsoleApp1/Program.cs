using ConsoleApp1.Models.UserModels;
using ConsoleApp1.Models.UserTransactionModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Program
    {
        //<------------Methods------------------------------------->
        //Virtual Method To ReadLine (Allows for override in testing. Will need multiple copies for multiple selections in testing.)
        public class InputRetriever
        {
            public virtual string GetInput()
            {
                return Console.ReadLine();
            }
        }

        //Create Account Method
        public List<User> CreateUser(List<User> _users)
        {
            InputRetriever input = new InputRetriever();
            User currentUser = new User();
            var users = _users;
            Console.WriteLine("Please enter your full email address");
            string emailResult = input.GetInput().Trim().ToLower();

            if (users.FindIndex(u => u.UserAcctName == emailResult) < 0)
            {
                currentUser.UserAcctName = emailResult;
                Console.WriteLine("Please create a password");
                string passResult1 = input.GetInput();

                Console.WriteLine("Confirm your password");
                string passResult2 = input.GetInput();

                if (passResult1 == passResult2)
                {
                }
                else
                {
                    while (passResult1 != passResult2)
                    {
                        Console.WriteLine("Sorry. Your passwords did not match. Please try again.");
                        Console.WriteLine("Please create a password");
                        passResult1 = input.GetInput();

                        Console.WriteLine("Confirm your password");
                        passResult2 = input.GetInput();
                    }
                }
                currentUser.UserAcctPassword = passResult1;
                currentUser.UserAcctId = users.Count + 1;
                users.Add(currentUser);
                return users;

            }
            else
            {
                Console.WriteLine("This user already exists. Press enter to restart.");
                input.GetInput();
                return users;
            }
        }

        //Log in Method. Also home of main menu. 
        public Tuple<List<User>,List<Transaction>> LogIn(List<User> _users, List<Transaction> _transactions)
        {
            User currentUser = new User();
            InputRetriever input = new InputRetriever();
            var users = _users;
            var transactions = _transactions;
            bool loggedIn = false;
            Console.Clear();
            Console.WriteLine("Type your email address to log in.");
            string result = input.GetInput().ToLower();
            if (users.FindIndex(u => u.UserAcctName == result) >= 0)
            {
                int attempts = 0;
                User attemptUser = users.Find(u => u.UserAcctName == result);
                string attemptPass;
                Console.WriteLine("Please enter your password");
                attemptPass = input.GetInput();

                while (attemptPass != attemptUser.UserAcctPassword && attempts < 3)
                {
                    Console.WriteLine("Sorry. That password does not match what we have on file. Please enter your password again.");
                    attemptPass = input.GetInput();
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
                input.GetInput();
                return new Tuple<List<User>, List<Transaction>>(users, transactions);
            }
            else
            {
                Console.WriteLine("Sorry. We could not find a user with that email address. Press Enter to try again.");
                input.GetInput();
                return new Tuple<List<User>, List<Transaction>>(users, transactions);
            }

        }

        //Start Menu (create account, log in and run main menu, or shut down program)
        public Tuple<bool, List<User>, List<Transaction>> Start(List<User> users, List<Transaction> transactions)
        {
            User currentUser = new User();
            Console.Clear();
            Console.WriteLine("Chose an option:\n1: Create New Account\n2: Log In\n3: Shut Down");
            InputRetriever input = new InputRetriever();
            int selection;
            bool success = Int32.TryParse(input.GetInput(), out selection);
            if (success)
            {
                if (selection == 1)
                {
                    users = CreateUser(users);
                    return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                }
                else if (selection == 2)
                {
                    var results = LogIn(users, transactions);
                    users = results.Item1;
                    transactions = results.Item2;
                    return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);         
                }
                else if (selection == 3)
                {
                    Console.WriteLine("Are you sure you want to shut down? Y/N?");
                    string result = input.GetInput().ToLower();
                    if (result == "y")
                    {
                        return new Tuple<bool, List<User>, List<Transaction>>(false, users, transactions);
                    } else
                    {
                        Console.WriteLine("Shut Down Cancelled. Press Enter to restart");
                        input.GetInput();
                        return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                    }

                }
                else {
                    Console.WriteLine("Please only enter numbers 1, 2, or 3. Press Enter to restart");
                    input.GetInput();
                    return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
                }

            }
            else
            {
                Console.WriteLine("Please only enter numbers 1, 2, or 3. Press Enter to restart");
                input.GetInput();
            }
            return new Tuple<bool, List<User>, List<Transaction>>(true, users, transactions);
        }
                    
        //Main Menu method once a User is logged in.
        public Tuple<bool, List<Transaction>> MainMenu(User user, List<Transaction> _transactions)
        {
            {
                List<Transaction> transactions = _transactions;
                InputRetriever input = new InputRetriever();
                Console.Clear();
                Console.WriteLine(
                   user.UserAcctName + " Logged In\n Options:  \n 1: Deposit\n 2: Withdraw\n 3: Balance Check\n 4: History\n 5: Logout");
                int selection;
                bool success = Int32.TryParse(input.GetInput(),out selection);
                if (success)
                {
                    if (selection == 1)
                    {
                        Transaction transaction = Deposit(user.UserAcctId);
                        if (transaction != null)
                        {
                            Console.WriteLine("Are you sure you want to deposit $" + transaction.Amt + "? Y/N");
                            string choice = input.GetInput().ToLower();
                            if (choice == "y")
                            {
                                transaction.Id = transactions.Count + 1;
                                transactions.Add(transaction);
                                Console.WriteLine("Deposit successful. Press enter.");
                                input.GetInput();
                                return new Tuple<bool, List<Transaction>>(true, transactions);
                            }
                            else
                            {
                                Console.WriteLine("Deposit cancelled. Press enter.");
                                input.GetInput();
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
                            Console.WriteLine("Are you sure you want to withdrawal $" + (transaction.Amt * -1) + "? Y/N");
                            string choice = input.GetInput().ToLower();
                            if (choice == "y")
                            {
                                if ((Balance(user.UserAcctId, transactions) + transaction.Amt) > 0)
                                {
                                    transaction.Id = transactions.Count + 1;
                                    transactions.Add(transaction);
                                    Console.WriteLine("Withdrawal successful. Press enter.");
                                    input.GetInput();
                                    return new Tuple<bool, List<Transaction>>(true, transactions);
                                }
                                else
                                {
                                    Console.WriteLine("Withdrawal cancelled. Balance too low. Press enter.");
                                    input.GetInput();
                                    return new Tuple<bool, List<Transaction>>(true, transactions);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Withdrawal cancelled. Press enter.");
                                input.GetInput();
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
                        input.GetInput();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                    else if (selection == 4)
                    {
                        History(user.UserAcctId, transactions);
                        Console.WriteLine("This is the end of your history. Press enter.");
                        input.GetInput();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                    else if (selection == 5)
                    {
                        return new Tuple<bool, List<Transaction>>(false, transactions);
                    }
                    else
                    {
                        Console.WriteLine("Please only input numbers 1, 2, 3, 4, or 5.");
                        input.GetInput();
                        return new Tuple<bool, List<Transaction>>(true, transactions);
                    }
                } else
                {
                    Console.WriteLine("Please only input numbers. Press enter to try again.");
                    input.GetInput();
                    return new Tuple<bool, List<Transaction>>(true, transactions);
                }
            }
        }
        
        //Seed Data Dummy User and Transactions. 
        public Tuple<List<User>, List<Transaction>> SeedData(List<User> _users, List<Transaction> _transactions)
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
                    Memo = "First Withdraw",
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
        public Transaction Deposit(int userId)
        {
            Console.WriteLine("How much would you like to deposit?");
            InputRetriever input = new InputRetriever();
            decimal inputAmt;
            bool success = decimal.TryParse(Console.ReadLine(),out inputAmt);

            if (success)
            {
                Console.WriteLine("Please add a note to this transaction.");
                string inputMemo = input.GetInput();

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
                input.GetInput();
                return null;
            }          
        }
        
        //Withdrawl Transaction Creation
       public Transaction Withdrawl(int userId)
        {
            Console.WriteLine("How much would you like to withdrawal?");
            decimal inputAmt;
            bool success = Decimal.TryParse(Console.ReadLine(), out inputAmt);
            InputRetriever input = new InputRetriever();

            if (success)
            {
                Console.WriteLine("You may add a note to this transaction.");
                string inputMemo = input.GetInput();

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
                input.GetInput();
                return null;
            }
            
        }
       
        //Check Balance Method - Takes in User and totals all their transactions. 
       public decimal Balance(int userId, List<Transaction> transactions)
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
        public void History(int userId, List<Transaction> transactions)
        {
            foreach(Transaction t in transactions)
            {
                if (t.UserId == userId)
                {
                    Console.WriteLine("Trans ID:" + t.Id + " Amt:$" + t.Amt + " Memo:" + t.Memo);
                }
                
            }
        }

    //----------------------Main Program---------------------------//
       public static void Main(string[] args)
        {
            var prg = new Program();
            List<User> userAccts = new List<User>();
            List<Transaction> transactions = new List<Transaction>();
            var seedResult = prg.SeedData(userAccts, transactions);
            userAccts = seedResult.Item1;
            transactions = seedResult.Item2;
            User currentUser = new User();
            bool running = true;

            while (running == true)
            {
                var results = prg.Start(userAccts, transactions);
                userAccts = results.Item2;
                transactions = results.Item3;
                running = results.Item1;
            }

            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }
}
