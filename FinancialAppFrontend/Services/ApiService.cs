using Common.Models;
using Common.ViewModels;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<TransactionTypeViewModel>> GetTransactionTypesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("FinanceApi");
            return await client.GetFromJsonAsync<List<TransactionTypeViewModel>>("api/TransactionType") ?? new List<TransactionTypeViewModel>();
        }
        catch (Exception)
        {
            return new List<TransactionTypeViewModel>();
        }
    }

    public async Task<List<TransactionViewModel>> GetTransactionsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("FinanceApi");
            return await client.GetFromJsonAsync<List<TransactionViewModel>>("api/Transaction");
        }
        catch (Exception)
        {
            return new List<TransactionViewModel>();
        }
    }

    public async Task<HttpResponseMessage> SaveTransactionAsync(TransactionViewModel transaction)
    {
        var client = _httpClientFactory.CreateClient("FinanceApi");
        HttpResponseMessage response;

        if (transaction.Id == 0)
        {
            response = await client.PostAsJsonAsync("api/Transaction", transaction);
        }
        else
        {
            response = await client.PutAsJsonAsync($"api/Transaction/{transaction.Id}", transaction);
        }

        return response;
    }

    public async Task<HttpResponseMessage> DeleteTransactionAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("FinanceApi");
        return await client.DeleteAsync($"api/Transaction/{id}");
    }

    public async Task<HttpResponseMessage> SaveTransactionTypeAsync(TransactionTypeViewModel type)
    {
        var client = _httpClientFactory.CreateClient("FinanceApi");
        HttpResponseMessage response;

        if (type.Id == 0)
        {
            response = await client.PostAsJsonAsync("api/TransactionType", type);
        }
        else
        {
            response = await client.PutAsJsonAsync($"api/TransactionType/{type.Id}", type);
        }

        return response;
    }

    public async Task<HttpResponseMessage> DeleteTransactionTypeAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("FinanceApi");
        return await client.DeleteAsync($"api/TransactionType/{id}");
    }

    public async Task<DailyReportViewModel?> GetDailyReportAsync(DateTime date)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("FinanceApi");
            return await client.GetFromJsonAsync<DailyReportViewModel>($"api/Report/DailyReport?date={date:MM-dd-yyyy}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<PeriodReportViewModel?> GetPeriodReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("FinanceApi");
            return await client.GetFromJsonAsync<PeriodReportViewModel>($"api/Report/PeriodReport?startDate={startDate:MM-dd-yyyy}&endDate={endDate:MM-dd-yyyy}");
        }
        catch
        {
            return null;
        }
    }
}
