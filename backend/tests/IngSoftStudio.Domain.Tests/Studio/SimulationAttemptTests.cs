using FluentAssertions;
using IngSoftStudio.Domain.Studio;
using Xunit;

namespace IngSoftStudio.Domain.Tests.Studio;

public sealed class SimulationAttemptTests
{
    [Fact]
    public void ConstructorCreatesAttemptWhenDataIsValid()
    {
        var userId = Guid.NewGuid();
        var attempt = new SimulationAttempt(userId, "risk-response", "mitigate", 100, "Excelente");

        attempt.Id.Should().NotBeEmpty();
        attempt.UserId.Should().Be(userId);
        attempt.Score.Should().Be(100);
        attempt.Level.Should().Be("Excelente");
    }

    [Fact]
    public void ConstructorRejectsOutOfRangeScore()
    {
        var action = () => new SimulationAttempt(Guid.NewGuid(), "scenario", "option", 101, "Invalid");

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}
