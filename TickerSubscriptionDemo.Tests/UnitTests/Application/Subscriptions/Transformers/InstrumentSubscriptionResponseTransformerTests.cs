using Newtonsoft.Json.Linq;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;

namespace TickerSubscriptionDemo.Tests.UnitTests.Application.Subscriptions.Transformers;

[Trait("Category", "UnitTests")]
public class InstrumentSubscriptionResponseTransformerTests
{
    private readonly InstrumentSubscriptionResponseTransformer transformerUnderTest;

    public InstrumentSubscriptionResponseTransformerTests()
    {
        this.transformerUnderTest = new InstrumentSubscriptionResponseTransformer();
    }

    [Fact]
    public void FromJson_WithNullResponse_ShouldThrow()
    {
        this.transformerUnderTest
            .Invoking(t => t.FromJson(response: null!))
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("response");
    }

    [Fact]
    public void FromJson_WithoutInstrumentName_ShouldThrow()
    {
        var invalidResponse = JToken.Parse(@"{
    ""channel"": ""ticker.MY_INSTRUMENT.100ms"",
    ""data"": {
      ""timestamp"": 1655683200000,
      //""instrument_name"": missing
    }
}");

        this.transformerUnderTest
            .Invoking(t => t.FromJson(invalidResponse))
            .Should().Throw<InvalidDataException>();
    }

    [Fact]
    public void FromJson_WithoutTimestamp_ShouldThrow()
    {
        var invalidResponse = JToken.Parse(@"{
    ""channel"": ""ticker.MY_INSTRUMENT.100ms"",
    ""data"": {
      //""timestamp"": missing,
      ""instrument_name"": ""MY_INSTRUMENT""
    }
}");

        this.transformerUnderTest
            .Invoking(t => t.FromJson(invalidResponse))
            .Should().Throw<InvalidDataException>();
    }

    [Fact]
    public void FromJson_WithValidData_ShouldReturnSuccessfully()
    {
        var response = JToken.Parse(@"{
    ""channel"": ""ticker.MY_INSTRUMENT.100ms"",
    ""data"": {
      ""timestamp"": 1655683200000,
      ""instrument_name"": ""ABC""
    }
}");

        var actualResponse = this.transformerUnderTest.FromJson(response);

        actualResponse.Name.Should().Be("ABC");
        actualResponse.Timestamp.Should().Be(new DateTime(2022, 06, 20, 00, 00, 00, DateTimeKind.Utc));
        actualResponse.Data.Should().Be(response.ToString());
    }
}