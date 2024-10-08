using CluedIn.ExternalSearch.Providers.Brreg.Clients;

namespace CluedIn.ExternalSearch.Brreg.Integration.Tests.Clients;

public class BrregClientTest
{
    [Fact]
    public void GetBrregOrganizationsByName_Success()
    {
        // Arrange
        var client = new BrregClient();

        // Act
        var result = client.GetBrregOrganizationsByName("Nordea Bank Abp filial i Norge").ToArray();

        // Assert
        Assert.True(result != null);
        Assert.Equal(20, result.Length);
    }

    [Fact]
    public void GetBrregOrganizationsByName_Failure_ReturnsEmpty()
    {
        // Arrange
        var client = new BrregClient();

        // Act
        var result = client.GetBrregOrganizationsByName(string.Empty).ToArray();

        // Assert
        Assert.True(result != null);
        Assert.Empty(result);
    }

    [Fact]
    public void GetBrregOrganizationByOrganizationNumber_Success()
    {
        // Arrange
        const string organizationNumber = "974760673";
        var client = new BrregClient();

        // Act
        var result = client.GetBrregOrganizationByOrganizationNumber(organizationNumber);

        // Assert
        Assert.True(result != null);
        Assert.Equal("REGISTERENHETEN I BRØNNØYSUND", result.Navn);
        Assert.Equal(organizationNumber, result.Organisasjonsnummer);
    }
}