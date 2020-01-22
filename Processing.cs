using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Data.Types;
using Palmer_Disposal___Management_System;

namespace Data
{
    //Pertains to management of customer data and saving
    public class Processing
    {
        //Encryption Data (Initailization Vector & Key for Encryption)
/*        public static string IV = "_littlePalmTree!";                       //Has to be 16 bytes or 128 Bits
        public static string Key = "alexDead@42&Ken-Is-A-Gay-Sad-Dad";      //Has to be 32 Bytes Long or 256 Bits*/

        //Save user preference (not customer data)
        public static void SavePrefs(string path, int themeIndex)
        {
            File.WriteAllText(path, themeIndex.ToString());
        }

        public static void LoadPrefs(string path, out int themeIndex)
        {
            themeIndex = 0;

            if(File.Exists(path))
            {
                themeIndex = int.Parse(File.ReadAllText(path));
            }
        }

        //Saves customer data to file
        public static void SaveData(List<Customer> customers, string path, bool confirmBox)
        {
            try
            {
                //Make dynamic array of strings containing each customer's data
                string[] customerData = new string[customers.Count];

                //Write customer data in specific format
                for (int i = 0; i < customers.Count; i++)
                {
                    //All user data in row seperated by commas
                    string temp = string.Format("{0}~{1}~{2}~{3}~{4}~{5}~{6}~{7}",
                        customers[i].accountNumber,
                        customers[i].name,
                        customers[i].phoneNumber,
                        customers[i].address,
                        customers[i].town,
                        customers[i].zip,
                        customers[i].comment,
                        customers[i].payments.Count);


                    //Gets number of payments and write them seperated by commas
                    for (int j = 0; j < customers[i].payments.Count; j++)
                    {
                        temp += string.Format("~{0}~{1}~{2}", customers[i].payments[j].timeDate, customers[i].payments[j].amount, customers[i].payments[j].comment);
                    }

                    //Encrypt Data
                    customerData[i] = temp;
                }

                //Write customer data to file
                File.WriteAllLines(path, customerData);


                // WIP - Encryption
/*                using (AesManaged aes = new AesManaged())
                {
                    byte[] encrypted = EncryptStringToBytes_Aes(File.ReadAllText(path), aes.Key, aes.IV);
                    File.WriteAllBytes(path, encrypted);
                }*/

                //Only display confirm box when explicily told
                if (confirmBox) System.Windows.MessageBox.Show("Saved Customer Data to " + path, "Saved", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Failed to Save Customer Data", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        //Load customer data to file, and appends it to cusomter array
        public static void LoadData(ref List<Customer> customers, string path)
        {

            //From file split each line into customer data
            if(File.Exists(path))
            {

                try
                {
                    //Create temp list of customers from file
                    List<Customer> loadedCustomers = new List<Customer>();

                    
                    //WIP - Decryption
/*                    string decrypted = null;
                    using(AesManaged aes = new AesManaged())
                    {
                        decrypted = DecryptStringFromBytes_Aes(File.ReadAllBytes(path), aes.Key, aes.IV);
                    }*/

                    //Get each line as indepenent strings
                    string[] lines = File.ReadAllLines(path);

                    //From lines (each customer) split sub-section of data to further parts
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //Split data when comma is reached
                        string[] data = lines[i].Split('~');


                        //Total number of payments from customer
                        List<Payment> tempPayments = new List<Payment>();


                        //Start Index (line number)
                        int start = 8;
                        int paymentOffset = 3;  //Amount of datatypes per payment

                        //Check each payment and add to total payments
                        for (int j = 0; j < (int.Parse(data[7]) * paymentOffset); j+=3)
                        {
                            Payment tempPayment = new Payment();

                            //Get values for temp payment
                            tempPayment.timeDate = data[start + j];
                            tempPayment.amount = data[start + j + 1];
                            tempPayment.comment = data[start + j + 2];

                            //Assign tempPayment to payments list
                            tempPayments.Add(tempPayment);
                        }

                        //From file data append to customer object
                        Customer tempCustomer = new Customer(
                            data[0],
                            data[1],
                            data[2],
                            data[3],
                            data[4],
                            data[5],
                            data[6],
                            tempPayments
                            );

                        //Once done with customer add customer to main customer list
                        customers.Add(tempCustomer);
                        customers[i].index = i;
                    }

                    Debug.WriteLine("Sucessfully Loaded Data");
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Exception: " + e.Message, "Failed to load customer data.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }        
            }
        }




        // Other //

        //Return available customers of specified list or array
        public static List<Customer> CustomerSearch(List<Customer> customers, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                //Temp Variables
                List<Customer> filteredCustomers = new List<Customer>();
                string[] split;
                List<int> indexes = new List<int>();


                //Split search term into parts
                split = search.Split(' ');

                //Check search words with customer data
                foreach (Customer customer in customers)
                {
                    //Check each customer data value from search term
                    for (int i = 0; i < split.Length; i++)
                    {
                        //Check for Customer Data from search words
                        if  (
                            customer.accountNumber.Contains(split[i])               || 
                            customer.name.ToUpper().Contains(split[i].ToUpper())    ||
                            customer.name.ToUpper().Contains(split[i].ToUpper())    ||
                            customer.address.ToUpper().Contains(split[i].ToUpper()) ||
                            customer.town.ToUpper().Contains(split[i].ToUpper())    ||
                            customer.phoneNumber.Contains(split[i])
                            )

                        //If any results come up add customer to list if they aren't apart of the list
                        {
                            if (!filteredCustomers.Contains(customer)) filteredCustomers.Add(customer);
                        }
                    }
                }

                return filteredCustomers;
            }

            return customers;
        }


        //Encryption -- Ref: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aesmanaged?redirectedfrom=MSDN&view=netframework-4.8
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if(plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("No Text");
            if(Key == null || Key.Length <= 0)
                throw new ArgumentNullException("No Key Given");
            if(IV == null || IV.Length <= 0)
                throw new ArgumentNullException("No IV Given");

            byte[] encrypted;

            //Create an AES Managed Object
            using(AesManaged aes = new AesManaged())
            {
                aes.Key = Key;
                aes.IV = IV;

                //Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                //Create the streams used for encryption
                using(MemoryStream msEncrypt = new MemoryStream())
                {
                    using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("No Text");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("No Key Given");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("No IV Given");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            System.Windows.MessageBox.Show(plaintext);

            return plaintext;
        }
    }
}
