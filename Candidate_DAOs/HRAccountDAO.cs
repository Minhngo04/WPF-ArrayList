using Candidate_BusinessObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Candidate_DAOs
{
    public class HRAccountDAO
    {
        private ArrayList hrAccounts;
        private static HRAccountDAO instance = null;
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hrAccounts.txt");

        public static HRAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HRAccountDAO();
                }
                return instance;
            }
        }

        // Constructor initializes by loading data from text file
        public HRAccountDAO()
        {
            hrAccounts = new ArrayList();
            LoadDataFromFile();
        }

        public Hraccount GetHraccountByEmail(string email)
        {
            return hrAccounts.Cast<Hraccount>().SingleOrDefault(n => n.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public List<Hraccount> GetHraccounts()
        {
            return hrAccounts.Cast<Hraccount>().ToList();
        }

        public bool AddHraccount(Hraccount hraccount)
        {
            if (GetHraccountByEmail(hraccount.Email) == null)
            {
                hrAccounts.Add(hraccount);
                SaveDataToFile();
                return true;
            }
            return false;
        }

        public bool DeleteHraccount(string email)
        {
            Hraccount hrAccount = GetHraccountByEmail(email);
            if (hrAccount != null)
            {
                hrAccounts.Remove(hrAccount);
                SaveDataToFile();
                return true;
            }
            return false;
        }

        public bool UpdateHraccount(Hraccount hraccount)
        {
            Hraccount existingAccount = GetHraccountByEmail(hraccount.Email);
            if (existingAccount != null)
            {
                hrAccounts.Remove(existingAccount);
                hrAccounts.Add(hraccount);
                SaveDataToFile();
                return true;
            }
            return false;
        }

        // Method to load data from a text file
        private void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split('\t'); // Sử dụng tab làm dấu phân cách
                    if (data.Length >= 4)
                    {
                        var hrAccount = new Hraccount
                        {
                            Email = data[0],
                            Password = data[1],
                            FullName = data[2],
                            MemberRole = int.Parse(data[3])
                        };
                        hrAccounts.Add(hrAccount);
                    }
                }
            }
        }


        // Method to save data to a text file
        private void SaveDataToFile()
        {
            var lines = hrAccounts.Cast<Hraccount>()
                                  .Select(a => $"{a.Email}\t{a.Password}\t{a.FullName}\t{a.MemberRole}");
            File.WriteAllLines(filePath, lines);
        }
    }
}
