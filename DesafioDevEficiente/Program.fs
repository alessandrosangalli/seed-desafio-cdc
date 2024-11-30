namespace DesafioDevEficiente
#nowarn "20"
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open DesafioDevEficiente.UseCases
open DesafioDevEficiente.Infra.Repositories

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        let connectionString = builder.Configuration.Item("ConnectionStrings:DefaultConnection")
        builder.Services.AddSingleton(connectionString)
        builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>() |> ignore
        builder.Services.AddSingleton<ICreateAuthorUseCase, CreateAuthorUseCase>() |> ignore
        builder.Services.AddControllers()

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
