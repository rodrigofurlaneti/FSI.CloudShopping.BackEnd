namespace FSI.CloudShopping.Domain.Enums;

/// <summary>
/// Represents the payment methods available in the system.
/// Focused on Brazilian payment methods.
/// </summary>
public enum PaymentMethod
{
    CreditCard = 0,
    DebitCard = 1,
    Pix = 2,
    BankSlip = 3,
    ManualTransfer = 4
}
