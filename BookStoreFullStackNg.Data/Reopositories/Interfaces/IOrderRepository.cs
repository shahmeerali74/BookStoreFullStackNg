using BookStoreFullStackNg.Data.DTOs.Order;

namespace BookStoreFullStackNg.Data.Reopositories.Interfaces;

public interface IOrderRepository
{
    Task CreateOrder(int userId,OrderCreateDto orderToCreate);
}
