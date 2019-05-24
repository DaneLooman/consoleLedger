using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models.UserTransactionModels
{
   public class Transaction
    {
        public int Id { get; set; }
        public string Memo { get; set; }
        public decimal Amt { get; set; }
        public int  UserId { get; set; }
    }
}
