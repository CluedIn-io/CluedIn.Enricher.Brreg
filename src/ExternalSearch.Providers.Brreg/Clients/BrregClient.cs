using CluedIn.ExternalSearch.Providers.Brreg.Models;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Brreg.Clients;

public class BrregClient
{
    private const string BrregApiBaseAddress = "https://data.brreg.no/enhetsregisteret/api/";

    private readonly HttpClient _httpClient = new();

    public IEnumerable<BrregOrganization> GetBrregOrganizationsByName(string name)
    {
        try
        {
            var response = _httpClient.GetAsync($"{BrregApiBaseAddress}enheter?navn={name}").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var organizations = JsonConvert.DeserializeObject<Root>(content)?.Embedded.Enheter;

            return organizations ?? Enumerable.Empty<BrregOrganization>();
        }
        catch (Exception)
        {
            // TODO: ROK: Log exception
            return Enumerable.Empty<BrregOrganization>();
        }
    }

    public BrregOrganization? GetBrregOrganizationByOrganizationNumber(string organizationNumber)
    {
        try
        {
            var response = _httpClient.GetAsync($"{BrregApiBaseAddress}enheter/{organizationNumber}").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var organization = JsonConvert.DeserializeObject<BrregOrganization>(content);

            return organization;
        }
        catch (Exception)
        {
            // TODO: ROK: Log exception
            return null;
        }
    }
}