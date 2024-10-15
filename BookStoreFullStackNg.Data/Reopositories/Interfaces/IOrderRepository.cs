using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Common;
using BookStoreFullStackNg.Data.DTOs.Order;

namespace BookStoreFullStackNg.Data.Reopositories.Interfaces;

public interface IOrderRepository
{
    Task CreateOrder(int userId,OrderCreateDto orderToCreate);
    Task<PagedList<Order>> UserOrder(int userId, UserOrdersQueryParameters queryParameters);
}
