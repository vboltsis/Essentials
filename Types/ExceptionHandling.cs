using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureExamples;

internal class ExceptionHandling
{
    public static void HandleException()
    {
        int[] numbers = { 1, 2, 3, 4, 5 };
        int divisor = 0; // Intentionally set to zero

        try
        {
            Console.WriteLine(numbers[2] / divisor);
        }
        catch (DivideByZeroException ex) when (divisor == 0)
        {
            Console.WriteLine("Error: Division by zero is not allowed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally");
        }
    }

    public static void HandleWithExceptionFilters()
    {
        var account = new BankAccount("Savings", 100); // Initial account type and balance

        try
        {
            account.Withdraw(150); // Attempt to withdraw more than the balance
        }
        catch (InsufficientFundsException ex) when (account.AccountType == "Savings")
        {
            Console.WriteLine("Error: Insufficient funds in your Savings account.");
        }
        catch (InsufficientFundsException ex) when (account.AccountType == "Checking")
        {
            Console.WriteLine("Error: Insufficient funds in your Checking account. Consider an overdraft protection.");
        }
        catch (AccountLockedException ex)
        {
            Console.WriteLine("Error: Your account is locked due to suspicious activity. Please contact support.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

public class BankAccount
{
    public string AccountType { get; }
    public double Balance { get; private set; }

    public BankAccount(string accountType, double initialBalance)
    {
        AccountType = accountType;
        Balance = initialBalance;
    }

    public void Withdraw(double amount)
    {
        if (amount > Balance)
        {
            throw new InsufficientFundsException();
        }

        // ... other logic
        Balance -= amount;
    }
}

public class InsufficientFundsException : Exception { }

public class AccountLockedException : Exception { }
