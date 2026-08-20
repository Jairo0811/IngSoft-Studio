using FluentAssertions;
using IngSoftStudio.Domain.Projects;
using Xunit;

namespace IngSoftStudio.Domain.Tests.Projects;

public sealed class ProjectTests
{
    [Fact]
    public void ConstructorCreatesDraftProjectWhenDataIsValid()
    {
        var ownerId = Guid.NewGuid();
        var project = new Project(ownerId, "  IngSoft Studio  ", "  Plataforma de ingeniería de software  ");

        project.Id.Should().NotBeEmpty();
        project.OwnerId.Should().Be(ownerId);
        project.Name.Should().Be("IngSoft Studio");
        project.Description.Should().Be("Plataforma de ingeniería de software");
        project.Status.Should().Be(ProjectStatus.Draft);
        project.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ConstructorRejectsEmptyName()
    {
        var action = () => new Project(Guid.NewGuid(), "   ", null);

        action.Should().Throw<ArgumentException>()
            .WithParameterName("name");
    }

    [Fact]
    public void ConstructorRejectsMissingOwner()
    {
        var action = () => new Project(Guid.Empty, "Proyecto", null);

        action.Should().Throw<ArgumentException>()
            .WithParameterName("ownerId");
    }

    [Fact]
    public void RenameTrimsNameAndUpdatesTimestamp()
    {
        var project = new Project(Guid.NewGuid(), "Original", null);

        project.Rename("  Nuevo nombre  ");

        project.Name.Should().Be("Nuevo nombre");
        project.UpdatedAtUtc.Should().NotBeNull();
    }
}
