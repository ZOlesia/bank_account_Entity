using System;
using System.Linq;
using System.Collections.Generic;
namespace bank_accounts.Models

{
    public class User
    {

        public int userid { get; set; }
        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<Transaction> transactions { get; set; }

        public User()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            transactions = new List<Transaction>();
        }

        public string Name {
            get {
                string name = this.first_name + " " + this.last_name;
                return name;
            }
        }

        public decimal balance {
            get{
                return this.transactions.Sum(a => a.amount);
            }
        }
    }
}