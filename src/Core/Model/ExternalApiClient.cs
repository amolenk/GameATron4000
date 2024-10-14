using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Amolenk.GameATron4000.Model;

public class ExternalApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public ExternalApiClient(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<ApiCallResult> GetAsync(string requestUri, Dictionary<string, string> claims)
    {
        var client = _httpClientFactory.CreateClient("Default");

        var token = GenerateJwtToken(claims);
        
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        try
        {
            var response = await client.SendAsync(request);
        
            return response.IsSuccessStatusCode 
                ? new ApiCallResult(HttpStatusCode.OK, await response.Content.ReadAsStringAsync()) 
                : new ApiCallResult(response.StatusCode, string.Empty);
        }
        catch (Exception e)
        {
            return new ApiCallResult(HttpStatusCode.InternalServerError, e.Message);
        }
    }
    
    private static string GenerateJwtToken(Dictionary<string, string> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims.Select(kv => new Claim(kv.Key, kv.Value)),
            expires: DateTime.Now.AddMinutes(10)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record ApiCallResult(HttpStatusCode StatusCode, string Content);