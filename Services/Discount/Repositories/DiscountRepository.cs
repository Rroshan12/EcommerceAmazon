using Discount.Entities;
using Dapper;
using Npgsql;

namespace Discount.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<Coupon> GetCouponByProductIdAsync(string productId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductId = @productId",
                new { productId });
        }

        public async Task<Coupon> GetCouponByIdAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE Id = @id",
                new { id });
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<Coupon>("SELECT * FROM Coupon");
        }

        public async Task<bool> CreateCouponAsync(Coupon coupon)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            coupon.CreatedAt = DateTime.UtcNow;
            coupon.UpdatedAt = DateTime.UtcNow;

            var result = await connection.ExecuteAsync(
                @"INSERT INTO Coupon (ProductId, ProductName, Description, Amount, CreatedAt, UpdatedAt)
                  VALUES (@ProductId, @ProductName, @Description, @Amount, @CreatedAt, @UpdatedAt)",
                coupon);
            return result > 0;
        }

        public async Task<bool> UpdateCouponAsync(Coupon coupon)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            coupon.UpdatedAt = DateTime.UtcNow;

            var result = await connection.ExecuteAsync(
                @"UPDATE Coupon SET ProductId = @ProductId, ProductName = @ProductName, 
                  Description = @Description, Amount = @Amount, UpdatedAt = @UpdatedAt
                  WHERE Id = @Id",
                coupon);
            return result > 0;
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE Id = @id",
                new { id });
            return result > 0;
        }
    }
}
