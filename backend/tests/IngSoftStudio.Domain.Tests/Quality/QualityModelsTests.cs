using FluentAssertions;
using IngSoftStudio.Domain.Quality;
using Xunit;

namespace IngSoftStudio.Domain.Tests.Quality;

public sealed class QualityModelsTests
{
    [Fact]
    public void RiskCalculatesScoreFromProbabilityAndImpact()
    {
        var risk = new Risk(Guid.NewGuid(), "Dependencia crítica", "Proveedor externo", RiskProbability.High, RiskImpact.High, "Proveedor alterno");
        risk.Score.Should().Be(9);
        risk.Status.Should().Be(RiskStatus.Open);
    }

    [Fact]
    public void TestCaseExecutionStoresResult()
    {
        var testCase = new TestCase(Guid.NewGuid(), null, "Login válido", "Usuario registrado", "Ingresar credenciales", "Acceso concedido");
        testCase.Execute(TestCaseStatus.Passed, "Acceso concedido");
        testCase.Status.Should().Be(TestCaseStatus.Passed);
        testCase.ExecutedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void DefectStartsOpen()
    {
        var defect = new Defect(Guid.NewGuid(), null, null, "Error de validación", "El formulario acepta vacío", DefectSeverity.High, DefectPriority.High);
        defect.Status.Should().Be(DefectStatus.Open);
    }
}
