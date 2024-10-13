
namespace BookStoreFullStackNg.Data.Constants;

public enum PaymentMethod
{
    CreditCard=1,
    DebitCard,
    PayPal,
    UPI,
    NetBanking
}

public enum PaymentStatus
{
    Pending=1,
    Completed,
    Failed,
    Refunded
}

public enum OrderStatus
{
    Pending=1,
    Confirmed,
    Shipped,
    Delivered,
    Canceled
}
