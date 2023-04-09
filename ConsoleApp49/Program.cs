﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03._04._23_HW1
{
    public class Account
    {
        private State state;
        private string owner;
        public Account(string owner)
        {
            this.owner = owner;
        }

        public double GetBalance()
        {
            return state.GetBalance();
        }

        public State GetState()
        {
            return state;
        }

        public void SetState(State state)
        {
            this.state = state;
            state.SetAccount(this);
        }

        public void Deposit(double amount)
        {
            state.Deposit(amount);
            Console.WriteLine($"Deposited {amount}$ -----");
            Console.WriteLine($"Balance {GetBalance()}$");
            Console.WriteLine($"Status {GetState().GetType().Name}");
            Console.WriteLine();
        }

        public void Withdraw(double amount)
        {
            bool result = state.Withdraw(amount);
            if (result)
            {
                Console.WriteLine($"Withdraw {amount}$ -----");
                Console.WriteLine($"Balance {GetBalance()}$");
                Console.WriteLine($"Status {GetState().GetType().Name}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Transaction failed.");
            }
        }

        public void PayInterest()
        {
            bool result = state.PayInterest();
            if (result)
            {
                Console.WriteLine("Interest Paid -----");
                Console.WriteLine($"Balance {GetBalance()}$");
                Console.WriteLine($"Status {GetState().GetType().Name}");
                Console.WriteLine();
            }
        }
    }
    public abstract class State
    {
        public Account account;
        public double balance;
        public double interest;
        public double lowerLimit;
        public double upperLimit;
        public Account GetAccount()
        {
            return account;
        }

        public void SetAccount(Account account)
        {
            this.account = account;
        }

        public double GetBalance()
        {
            return balance;
        }

        public void SetBalance(double balance)
        {
            this.balance = balance;
        }

        public abstract void Deposit(double amount);

        public abstract bool Withdraw(double amount);

        public abstract bool PayInterest();
    }
    public class SilverState : State
    {
        public SilverState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public SilverState(State state) : this(state.GetBalance(), state.GetAccount())
        {
            Initialize();
            StateChangeCheck();
        }

        private void Initialize()
        {
            interest = 0.1;
            lowerLimit = 0.0;
            upperLimit = 1000.0;
        }

        private void StateChangeCheck()
        {
            if (balance < lowerLimit)
            {
                account.SetState(new RedState(this));
            }
            else if (balance >= upperLimit)
            {
                account.SetState(new GoldState(this));
            }
        }

        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }

        public override bool Withdraw(double amount)
        {
            balance -= amount;
            StateChangeCheck();
            return true;
        }
        public override bool PayInterest()
        {
            balance += interest * balance;
            StateChangeCheck();
            return true;
        }
    }
    public class RedState : State
    {
        public RedState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public RedState(State state) : this(state.GetBalance(), state.GetAccount())
        {
            Initialize();
            StateChangeCheck();
        }
        private void Initialize()
        {
            interest = 0.0;
            lowerLimit = -100.0;
            upperLimit = 0.0;
        }

        private void StateChangeCheck()
        {
            if (balance >= lowerLimit && balance < upperLimit)
            {
                account.SetState(new SilverState(this));
            }
            else if (balance >= upperLimit)
            {
                account.SetState(new GoldState(this));
            }
        }

        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }

        public override bool Withdraw(double amount)
        {
            Console.WriteLine("No funds available for withdrawal!\n");
            return false;
        }

        public override bool PayInterest()
        {
            Console.WriteLine("No interest is paid!\n");
            return false;
        }
    }
    public class GoldState : State
    {
        public GoldState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public GoldState(State state) : this(state.GetBalance(), state.GetAccount())
        {
            Initialize();
            StateChangeCheck();
        }

        private void Initialize()
        {
            interest = 0.07;
            lowerLimit = 1000.0;
            upperLimit = 10000000.0;
        }
        private void StateChangeCheck()
        {
            if (balance < lowerLimit)
            {
                account.SetState(new RedState(this));
            }
            else if (balance >= upperLimit)
            {
                account.SetState(new SilverState(this));
            }
        }
        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }

        public override bool Withdraw(double amount)
        {
            if (balance - amount < lowerLimit)
            {
                return false;
            }
            balance -= amount;
            StateChangeCheck();
            return true;
        }

        public override bool PayInterest()
        {
            balance += interest * balance;
            StateChangeCheck();
            return true;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}