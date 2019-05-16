using ConsoleApp1.Models.UserModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
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
                            Console.WriteLine("Please enter a passowrd");
                            string passResult1 = Console.ReadLine();

                            Console.WriteLine("Please re-enter a passowrd");
                            string passResult2 = Console.ReadLine();

                            if (passResult1 == passResult2) {
                            }
                            else {
                                while (passResult1 != passResult2) { 
                                    Console.WriteLine("Sorry. Your passowrd did not match. Please try again.");
                                    Console.WriteLine("Please enter a passowrd");
                                    passResult1 = Console.ReadLine();

                                    Console.WriteLine("Please re-enter a passowrd");
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
                Console.Clear();
                Console.WriteLine(
                   currentUser.UserAcctName + " Logged In\n Options:  \n 1: Deposit\n 2: Withdraw\n 3: Balance Check\n 4: History\n 5: Logout");
                int selection = Convert.ToInt32(Console.ReadLine());
                if (selection == 1) {
                    Console.WriteLine("You did a Deposit");
                } else if (selection == 2){
                    Console.WriteLine("You withdrew");
                } else if (selection == 3) {
                    Console.WriteLine("This is your balance");
                } else if (selection == 4) {
                    Console.WriteLine("This is your history");
                } else if (selection == 5) {
                    currentUser = null;
                    loggedIn = false;
                    Console.Clear();
                } else {}
            }
            Console.WriteLine("You have logged out");
            Console.Read();
        }
    }
}
