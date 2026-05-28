using MudBlazorClient.Models;
using System.Net.Http.Json;

namespace MudBlazorClient.Services;

public class ApiService(HttpClient http)
{
    // ── GET ──────────────────────────────────────────────────────────

    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        try { return await http.GetFromJsonAsync<List<CustomerDto>>("api/customers") ?? []; }
        catch { return []; }
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        try { return await http.GetFromJsonAsync<List<ProductDto>>("api/products") ?? []; }
        catch { return []; }
    }

    public async Task<List<SystemDto>> GetSystemsAsync(int? customerId = null, int? productId = null)
    {
        var query = "api/systems";
        var parts = new List<string>();
        if (customerId.HasValue) parts.Add($"customerId={customerId}");
        if (productId.HasValue) parts.Add($"productId={productId}");
        if (parts.Count > 0) query += "?" + string.Join("&", parts);

        try { return await http.GetFromJsonAsync<List<SystemDto>>(query) ?? []; }
        catch { return []; }
    }

    public async Task<List<SystemVersionDto>> GetSystemVersionsAsync(int systemId)
    {
        try { return await http.GetFromJsonAsync<List<SystemVersionDto>>($"api/system-versions?systemId={systemId}") ?? []; }
        catch { return []; }
    }

    public async Task<List<SegmentDto>> GetSegmentsAsync(int systemVersionId)
    {
        try { return await http.GetFromJsonAsync<List<SegmentDto>>($"api/segments?systemVersionId={systemVersionId}") ?? []; }
        catch { return []; }
    }

    public async Task<List<ReleaseTypeDto>> GetReleaseTypesAsync()
    {
        try { return await http.GetFromJsonAsync<List<ReleaseTypeDto>>("api/release-types") ?? []; }
        catch { return []; }
    }

    public async Task<List<ReleaseRequestDto>> GetReleaseRequestsAsync()
    {
        try { return await http.GetFromJsonAsync<List<ReleaseRequestDto>>("api/release-requests") ?? []; }
        catch { return []; }
    }

    public async Task<bool> CreateReleaseRequestAsync(string? location, DateTime? releaseDate, int? releaseTypeId, string? notes)
    {
        var response = await http.PostAsJsonAsync("api/release-requests", new { location, releaseDate, releaseTypeId, notes });
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CsciDto>> GetCscisAsync(int segmentId)
    {
        try { return await http.GetFromJsonAsync<List<CsciDto>>($"api/cscis?segmentId={segmentId}") ?? []; }
        catch { return []; }
    }

    // ── POST ─────────────────────────────────────────────────────────

    public async Task<bool> CreateCustomerAsync(string name, string? abbreviation)
    {
        var response = await http.PostAsJsonAsync("api/customers", new { name, abbreviation });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateProductAsync(string name, string? description)
    {
        var response = await http.PostAsJsonAsync("api/products", new { name, description });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateSystemAsync(int customerId, int productId, string? pocName, string? pocEmail, string? pocPhone)
    {
        var response = await http.PostAsJsonAsync("api/systems", new { customerId, productId, pocName, pocEmail, pocPhone });
        return response.IsSuccessStatusCode;
    }
}
