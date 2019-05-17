using ConsoleApp1.Models.UserModels;
using ConsoleApp1.Models.UserTransactionModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static bool MainMenu(User user)
        {
            {
                Console.Clear();
                Console.WriteLine(
                   user.UserAcctName + " Logged In\n Options:  \n 1: Deposit\n 2: Withdraw\n 3: Balance Check\n 4: History\n 5: Logout");
                int selection = Convert.ToInt32(Console.ReadLine());
                if (selection == 1)
                {
                    Console.WriteLine("You did a Deposit. Press Enter.");
                    Console.ReadLine();
                    return true;
                }
                else if (selection == 2)
                {
                    Console.WriteLine("You withdrew. Press Enter.");
                    Console.ReadLine();
                    return true;
                }
                else if (selection == 3)
                {
                    Console.WriteLine("This is your balance. Press Enter.");
                    Console.ReadLine();
                    return true;
                }
                else if (selection == 4)
                {
                    Console.WriteLine("This is your history. Press Enter.");
                    Console.ReadLine();
                    return true;
                }
                else if (selection == 5)
                {                
                    return false;
                }
                else
                {
                    Console.WriteLine("Please only input numbers.");
                    Console.ReadLine();
                    return true;
                }
            }
        }

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






        static void Main(string[] args)
        {
            List<User> userAccts = new List<User>();
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
                loggedIn = MainMenu(currentUser);
            }

            Console.WriteLine("You have logged out");
            Console.Read();
        }
    }
}
