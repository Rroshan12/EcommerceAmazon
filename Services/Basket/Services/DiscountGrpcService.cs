using Grpc.Net.Client;
using Grpc.Core;

namespace Basket.Services
{
    public class CouponReplyDto
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }

    public interface IDiscountGrpcService
    {
        Task<CouponReplyDto> GetDiscountAsync(string productId);
    }

    public class DiscountGrpcService : IDiscountGrpcService
    {
        private readonly GrpcChannel _channel;
        private readonly ILogger<DiscountGrpcService> _logger;
        private readonly dynamic _client;

        public DiscountGrpcService(IConfiguration configuration, ILogger<DiscountGrpcService> logger)
        {
            _logger = logger;
            var discountUrl = configuration.GetValue<string>("GrpcSettings:DiscountUrl") ?? "http://localhost:7166";
            
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _channel = GrpcChannel.ForAddress(discountUrl, new GrpcChannelOptions { HttpHandler = handler });
            
            // Dynamically create gRPC client using reflection
            var clientType = Type.GetType("Discount.Protos.DiscountService+DiscountServiceClient");
            if (clientType == null)
            {
                throw new InvalidOperationException("Discount.Protos.DiscountService client not found. Ensure Discount proto files are compiled.");
            }
            _client = Activator.CreateInstance(clientType, _channel);
        }

        public async Task<CouponReplyDto> GetDiscountAsync(string productId)
        {
            try
            {
                _logger.LogInformation("Calling Discount gRPC service for ProductId: {ProductId}", productId);
                
                // Create request object dynamically
                var requestType = Type.GetType("Discount.Protos.GetDiscountRequest");
                var request = Activator.CreateInstance(requestType);
                requestType.GetProperty("ProductId").SetValue(request, productId);

                // Call gRPC method
                var method = _client.GetType().GetMethod("GetDiscountAsync");
                var response = await (dynamic)method.Invoke(_client, new[] { request, null });
                
                if (response == null)
                {
                    _logger.LogWarning("Discount gRPC service returned null for ProductId: {ProductId}", productId);
                    return null;
                }

                _logger.LogInformation($"Successfully retrieved discount for ProductId: {productId}, Amount: {response.Amount}%");
                
                return new CouponReplyDto
                {
                    Id = response.Id,
                    ProductId = response.ProductId,
                    ProductName = response.ProductName,
                    Description = response.Description,
                    Amount = response.Amount
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to call Discount gRPC service for ProductId: {ProductId}. Returning null.", productId);
                return null;
            }
        }
    }
}




