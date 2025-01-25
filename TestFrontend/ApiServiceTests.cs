using System.Net;
using System.Net.Http.Json;
using Common.Models;
using Moq;
using Moq.Protected;

public class ApiServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly ApiService _apiService;

    public ApiServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        var client = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://example.com/")
        };

        _httpClientFactoryMock.Setup(f => f.CreateClient("FinanceApi")).Returns(client);

        _apiService = new ApiService(_httpClientFactoryMock.Object);
    }

    [Fact]
    public async Task GetTransactionTypesAsync_ReturnsTransactionTypes()
    {
        // Arrange
        var transactionTypes = new List<TransactionTypeViewModel>
        {
            new TransactionTypeViewModel { Id = 1, Name = "Income" },
            new TransactionTypeViewModel { Id = 2, Name = "Expense" }
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(transactionTypes)
            });

        // Act
        var result = await _apiService.GetTransactionTypesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Income", result[0].Name);
        Assert.Equal("Expense", result[1].Name);
    }

    [Fact]
    public async Task GetTransactionsAsync_ReturnsTransactions()
    {
        // Arrange
        var transactions = new List<TransactionViewModel>
        {
            new TransactionViewModel { Id = 1, Amount = 100, Date = DateTime.Today, TransactionTypeId = 1, Comment = "Salary" },
            new TransactionViewModel { Id = 2, Amount = 50, Date = DateTime.Today, TransactionTypeId = 2, Comment = "Groceries" }
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(transactions)
            });

        // Act
        var result = await _apiService.GetTransactionsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Salary", result[0].Comment);
        Assert.Equal("Groceries", result[1].Comment);
    }

    [Fact]
    public async Task SaveTransactionAsync_PostsNewTransaction()
    {
        // Arrange
        var transaction = new TransactionViewModel { Id = 0, Amount = 100, Date = DateTime.Today, TransactionTypeId = 1, Comment = "Salary" };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created });

        // Act
        var response = await _apiService.SaveTransactionAsync(transaction);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task SaveTransactionAsync_UpdatesExistingTransaction()
    {
        // Arrange
        var transaction = new TransactionViewModel { Id = 1, Amount = 150, Date = DateTime.Today, TransactionTypeId = 1, Comment = "Bonus" };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

        // Act
        var response = await _apiService.SaveTransactionAsync(transaction);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTransactionAsync_DeletesTransaction()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

        // Act
        var response = await _apiService.DeleteTransactionAsync(1);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
