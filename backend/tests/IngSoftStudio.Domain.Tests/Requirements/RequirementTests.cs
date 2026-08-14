using FluentAssertions;
using IngSoftStudio.Domain.Requirements;
using Xunit;

namespace IngSoftStudio.Domain.Tests.Requirements;

public sealed class RequirementTests
{
    [Fact]
    public void ConstructorCreatesProposedRequirementWithTraceabilityData()
    {
        var projectId = Guid.NewGuid();

        var requirement = new Requirement(
            projectId,
            "Autenticación",
            "El sistema debe permitir iniciar sesión.",
            RequirementType.Functional,
            RequirementPriority.Must,
            "Dado un usuario válido, cuando inicia sesión, entonces accede al sistema.",
            "Entrevista con stakeholder");

        requirement.ProjectId.Should().Be(projectId);
        requirement.Status.Should().Be(RequirementStatus.Proposed);
        requirement.Priority.Should().Be(RequirementPriority.Must);
        requirement.AcceptanceCriteria.Should().NotBeNullOrWhiteSpace();
        requirement.Source.Should().Be("Entrevista con stakeholder");
    }

    [Fact]
    public void ConstructorRejectsEmptyProjectId()
    {
        var action = () => new Requirement(Guid.Empty, "Título", "Descripción", RequirementType.Functional, RequirementPriority.Must);

        action.Should().Throw<ArgumentException>().WithParameterName("projectId");
    }

    [Fact]
    public void ChangeStatusUpdatesLifecycleState()
    {
        var requirement = new Requirement(Guid.NewGuid(), "Título", "Descripción", RequirementType.UserStory, RequirementPriority.Should);

        requirement.ChangeStatus(RequirementStatus.Approved);

        requirement.Status.Should().Be(RequirementStatus.Approved);
        requirement.UpdatedAtUtc.Should().NotBeNull();
    }
}
