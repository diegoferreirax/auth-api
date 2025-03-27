using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace Tests.Architecture;

public class ArchitectureTests
{
    //private static readonly Architecture Architecture =
    //    new ArchitectureBuilder().AddAssemblies(typeof(Startup).Assembly).Build();

    //[Fact]
    //public void ApplicationLayer_ShouldNotReferenceInfrastructureLayer()
    //{
    //    var applicationNamespace = "MeuProjeto.Application";
    //    var infrastructureNamespace = "MeuProjeto.Infrastructure";

    //    var rule = Types().That().ResideInNamespace(applicationNamespace)
    //                      .Should().NotDependOnAny(infrastructureNamespace);

    //    rule.Check(Architecture);
    //}

    //private static readonly ArchUnitNET.Domain.Architecture Architecture =
    //        new ArchLoader().LoadAssemblies(
    //            Assembly.LoadFrom("AuthApi.WebApi"),
    //            Assembly.LoadFrom("AuthApi.Application"),
    //            Assembly.LoadFrom("AuthApi.Shared"))
    //        .Build();

    //private readonly IObjectProvider<IType> apiAssembly =
    //    Types().That().ResideInAssembly("AuthApi").As("AuthApi");

    //private readonly IObjectProvider<IType> applicationAssembly =
    //    Types().That().ResideInAssembly("Application").As("Application");

    //private readonly IObjectProvider<IType> sharedAssembly =
    //    Types().That().ResideInAssembly("Shared").As("Shared");

    //[Fact]
    //public void APICannotAccessApplication()
    //{
    //    var regraCamadas = Types()
    //        .That()
    //        .Are(applicationAssembly)
    //        .Should()
    //        .NotDependOnAny(apiAssembly)
    //        .Because("A camada da API nao pode acessar a aplicação");

    //    regraCamadas.Check(Architecture);
    //}

}
