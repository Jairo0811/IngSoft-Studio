using FluentAssertions;
using IngSoftStudio.Domain.Projects;
using Xunit;

namespace IngSoftStudio.Domain.Tests.Projects;

public sealed class ProjectTests
{
    [Fact]
    public void Constructor_ShouldCreateDraftProject_WhenDataIsValid()
    {
        var project = new Project("  IngSoft Studio  ", "  Plataforma de ingeniería de software  ");

        project.Id.Should().NotBeEmpty();
        project.Name.Should().Be("IngSoft Studio");
        project.Description.Should().Be("Plataforma de ingeniería de software");
        project.Status.Should().Be(ProjectStatus.Draft);
        project.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyName()
    {
        var action = () => new Project("   ", null);

        action.Should().Throw<ArgumentException>()
            .WithParameterName("name");
    }

    [Fact]
    public void Rename_ShouldTrimName_AndUpdateTimestamp()
    {
        var project = new Project("Original", null);

        project.Rename("  Nuevo nombre  ");

        project.Name.Should().Be("Nuevo nombre");
        project.UpdatedAtUtc.Should().NotBeNull();
    }
}
