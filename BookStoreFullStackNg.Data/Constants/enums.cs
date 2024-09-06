
namespace BookStoreFullStackNg.Data.Constants;

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    UPI,
    NetBanking
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Canceled
}
