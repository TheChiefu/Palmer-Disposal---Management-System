using System;
using System.Collections.Generic;

namespace Data
{
    //Holds all data types (Customers, Payments, etc)
    namespace Types
    {
        public class Customer
        {
            //Constructor function

            //General Info
            public string accountNumber { get; set; }
            public string name { get; set; }

            public string comment { get; set; }

            //Address Data
            public string address { get; set; }


            public string town { get; set; }

            public string zip { get; set; }

            //Other
            public string phoneNumber { get; set; }

            public List<Payment> payments;
            public int index { get; set; }

            // Constructors //


            //Default Constructor
            public Customer()
            {
                this.accountNumber = "?";
                this.name = "";
                this.address = "";
                this.town = "";
                this.zip = "";
                this.phoneNumber = "";
                this.comment = "";
                this.payments = new List<Payment>();
            }

            //Given Information Constructor
            public Customer(string accountNumber, string name, string phoneNumber, string address, string town, string zip, string comment, List<Payment> payments)
            {
                this.accountNumber = accountNumber;
                this.name = name;
                this.address = address;
                this.town = town;
                this.zip = zip;
                this.phoneNumber = phoneNumber;
                this.comment = comment;
                this.payments = payments;
            }
        }

        // Payment struct that contains a time/date and amount paid
        // Used for payment history logs in customers
        public class Payment
        {
            //In format: DD-MM-YYYY hh:mm::ss
            public string timeDate { get; set; }

            //Payment amount
            public string amount { get; set; }

            //Comment
            public string comment { get; set; }

            //Default constructor
            public Payment()
            {
                this.timeDate = "";
                this.amount = "";
                this.comment = "";
            }

            //Specified Date
            public Payment(string date)
            {
                this.timeDate = date;
                this.amount = "";
                this.comment = "";
            }

            //Specified Amount
            public Payment(decimal amount)
            {
                this.timeDate = DateTime.Now.ToString();
                this.amount = "";
                this.comment = "";
            }

            //Specified Details
            public Payment(string date, decimal amount, string comment)
            {
                this.timeDate = date;
                this.amount = "";
                this.comment = comment;
            }
        }
    }
}
