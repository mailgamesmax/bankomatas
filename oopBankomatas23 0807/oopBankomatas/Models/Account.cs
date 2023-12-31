﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace oopBankomatas.Models
{
    internal class Account : Client //, Iidentify
    {
        public Account() { }
        public Account(string name, string lastName) : base(name, lastName) {}
        public Account(string name, string lastName, string clientID) : this(name, lastName)
        {
            this.ClientID = clientID;
        }

/*        public Account(string name, string lastName, Dictionary<string, string> accoutID) : this(name, lastName)
        {
            AccountID = accoutID;
        }
*/
 
        

/*        public void ActionMeniu(Account account) 
        {
*//*        string? actionDescription = null;
        double actionValue = 0;*//*
        string? requestForAnotherAction;
            do
            {
                Console.Write("pasirink veiskmą ");
                //actionDescription = ActionManager(account, ref actionDescription);
                ActionManager(account);
                Console.WriteLine("dar kas nors? (+) ");
                requestForAnotherAction = Console.ReadLine();
            }

            while (requestForAnotherAction == "+");
        }*/

        public void ActionReport(Account account) 
        {
            int i = 1;
            foreach (var action in  account.AccountActions)
            {
                Console.WriteLine("operacijos nr: " + i);
                foreach (var currentA in action)
                {
                Console.WriteLine("turinys " + currentA.Key);
                Console.WriteLine("suma " + currentA.Value);
                Console.WriteLine();
                }
                i++;
            }
            Console.WriteLine($"Likutis: {account.Balance}");
        }


    public virtual Account CreateAccount(Client client)
    {

        string userName = client.Name;
        string userLastName = client.LastName;
        string clientID = client.ClientID;
        bool isCoupeledClientIdAndPassw = false;
        do
        {

            Console.Write("sukurk slaptiką: ");
            string inputPassw = "abc123"; // pakeisti i ivesti

            isCoupeledClientIdAndPassw = AutorisationCell.TryAdd(clientID, inputPassw); //turi būti vienas dict......
            if (isCoupeledClientIdAndPassw) { Console.WriteLine($"isimink slapika.....->  {inputPassw}"); }
            else { Console.WriteLine("kažkas ne OK su slapiku"); }
        }
        while (!isCoupeledClientIdAndPassw);
        //Dictionary<string, string> accountID = new Dictionary<string, string>() {{ clientID, inputPassw }}; //turi būti vienas dict......
        Account newAccount = new Account(userName, userLastName, clientID);
        UpdateAllAccounts(newAccount);
            Console.WriteLine($"{newAccount.Name} - account sukurtas");
            var newCreditCard = new CreditCard();
            newCreditCard = newCreditCard.MakeNewCard(newAccount);
            Console.WriteLine($"{newCreditCard.UniqNr} - kortelė sukurta");
            return newAccount;
    }

    /*        public bool Verification()
            {
                Console.WriteLine("duok passw.....");
                string inputedPasw = "abc123"; return true; 

            }*/
    public Account CreditCardAccountVerification(CreditCard indicatedCard)
    {
        Account indicatedAcc = AllAccounts.SingleOrDefault(acc => acc.ClientID == indicatedCard.ClientID);
        bool passwExist = AutorisationCell.TryGetValue(indicatedAcc.ClientID, out var getPassw);
            Console.WriteLine("issaugotas passw -> \n" + getPassw); // savikontrolei
            string? checkThisPassw;
        if (passwExist)
        {
            int i = 1;
            do
            {
                Console.WriteLine("duok pasw...");
                    checkThisPassw = Console.ReadLine();
                if (checkThisPassw == getPassw)
                {
                    Console.WriteLine("acc passw ok"); //for test only
                    return indicatedAcc;
                }
                else
                {
                    Console.WriteLine("acc passw wrong"); //for test only
                                                          //return false;
                    i++;
                }
            }
            while (checkThisPassw != getPassw && i < 4);
            indicatedAcc = null;
            return indicatedAcc;
        }
        else
        {
            Console.WriteLine("kortelės klaida (nėra passw)");
            indicatedAcc = null;
            return indicatedAcc;
        }
    }

        public List<Account> UpdateAllAccounts(Account newAccount)
        {
            AllAccounts.Add(newAccount);
            return AllAccounts;
        }

        //public string ActionManager(Account account, ref string availibleActionDescription)
        public void ActionManager(Account account)
        {
            int choosedAction = 0;
            string check_Balance = "Patikrinti lėšų likutį"; //1
            string check_LastActions = "Peržiūrėti transakcijas (5 paskutinės"; //2
            string take_Cash = "Išimti pinigų"; //0
            string put_Cash = "ĮNEŠTI pinigų"; //0
            string userDisconnect = "Grąžinti kortelę"; //0
            string availibleActionDescription;

            Console.WriteLine($"Pasirinkite veiksmą: \n 1 - {check_Balance} \n 2 -{check_LastActions} \n\n 3 - {take_Cash} \n 4 - {put_Cash} \n\n 0 - {userDisconnect} \n");
            choosedAction = Convert.ToInt16(Console.ReadLine());
            Dictionary<string, double> actionDetails = new Dictionary<string, double>();

            switch (choosedAction)
            {
                case 1:
                    availibleActionDescription = check_Balance;
                    Console.WriteLine("esamas likutis " +account.Balance+ "euriuku");
                    //return availibleActionDescription;
                    break;
                case 2:
                    availibleActionDescription = check_LastActions;
                    ActionReport(account);
                    //return availibleActionDescription;
                    break;
                case 3:
                    availibleActionDescription = take_Cash;

                    Console.Write("Įvesk sumą (įšimti -max 1000e): ");
                    double actionValue = Convert.ToDouble(Console.ReadLine());
                   // Dictionary<string, double> actionDetails = new Dictionary<string, double>();

                    if (account.Balance - actionValue < 0 || actionValue > 1000)
                    {
                        Console.WriteLine("ką čia bandai apgaut seni?....");
                    }
                    else
                    {
                        if (account.AccountActions.Count > 4) account.AccountActions.RemoveAt(0);
                        actionDetails.Add(availibleActionDescription, actionValue);
                        account.AccountActions.Add(actionDetails);
                        account.Balance -= actionValue;
                        Console.WriteLine($"{availibleActionDescription} <- įvykdyta\n{actionValue} <- operacijos suma\n{account.Balance} <- ESAMAS LIKUTIS");
                    }
                    //return availibleActionDescription;
                    break;
                case 4:                
                    availibleActionDescription = put_Cash;
                    Console.Write("Įvesk sumą: ");
                    actionValue = Convert.ToDouble(Console.ReadLine());
                    //Dictionary<string, double> actionDetails = new Dictionary<string, double>();
                    if (actionValue < 0)
                    {
                        Console.WriteLine("ką čia bandai apgaut seni?....");
                    }
                    else
                    {
                        if (account.AccountActions.Count > 4) account.AccountActions.RemoveAt(0);
                        actionDetails.Add(availibleActionDescription, actionValue);
                        account.AccountActions.Add(actionDetails);
                        account.Balance += actionValue;
                        Console.WriteLine($"{availibleActionDescription} <- įvykdyta\n{actionValue} <- operacijos suma\n{account.Balance} <- ESAMAS LIKUTIS");
                    }
                    //return availibleActionDescription;
                    break;
                case 0:
                    availibleActionDescription = userDisconnect;
                    Console.WriteLine("chaos chikitos  : )))))) ");
                    Environment.Exit(0);

                    //return availibleActionDescription;
                    break;
                default:
                    Console.WriteLine("chaos chikitos  : )))))) ");
                    Environment.Exit(0);
                    break;
            }
            //return availibleActionDescription = "nepasirinkta";
        }


        // testing begin
        public static Account CreateAccForTest(Client client, string testpassw) //testams
        {
            string userName = client.Name;
            string userLastName = client.LastName;
            string clientID = client.ClientID;
            string inputPassw = testpassw;
            AutorisationCell.TryAdd(clientID, inputPassw); //turi būti vienas dict......
            Console.WriteLine($"isimink slapika.....->  {inputPassw}");
            Account newAccount = new Account(userName, userLastName, clientID);
            AllAccounts.Add(newAccount);

            var newCreditCard = new CreditCard();
            newCreditCard.MakeNewCard(newAccount);
            Console.WriteLine($"{newCreditCard.UniqNr} - kortelė sukurta");
            return newAccount;
        }
        // testing END

        private static Dictionary<string, string> AutorisationCell { get; set; } = new Dictionary<string, string>(); // clientID+passw      
        public double Balance { get; set; } = 1500; //testinis likutis 1500
        
        //public string AvailibleActions { get; set; }
        public double ActionValue { get; set; }

        public List<Dictionary<string, double>> AccountActions { get; set; } = new List<Dictionary<string, double>>(5);
        public static List<Account> AllAccounts { get; set; } = new List<Account>();
    }
}
