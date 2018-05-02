using System;
namespace bank_accounts.Models
{
    public class Transaction
    {
        public int transactionid { get; set; }
        public decimal amount { get; set; }
        public DateTime trans_date { get; set; }
        public DateTime trans_updated_at { get; set; }
        public int userid { get; set; }
        public User user { get; set; }

        public Transaction()
        {
            this.trans_date = DateTime.Now;
            this.trans_updated_at = DateTime.Now;
        }
    }
}