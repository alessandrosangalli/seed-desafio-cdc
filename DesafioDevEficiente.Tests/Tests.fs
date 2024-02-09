module Tests

open System
open Xunit
open DesafioDevEficiente.Controllers
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc
open AccidentalFish.FSharp.Validation

let DisposableStub = {
    new IDisposable with
        member x.Dispose() = ()
}

let LoggerStub = {
    new ILogger<AuthorController> with
        member x.BeginScope<'TState>(state: 'TState) = DisposableStub
        member x.IsEnabled(logLevel: LogLevel) = true
        member x.Log<'TState>(logLevel: LogLevel, eventId: EventId, state: 'TState, ``exception`` : exn, formatter: System.Func<'TState,exn,string>) = ()}

[<Fact>]
let ``Email can't be empty`` () =
    let createAuthorDto = {
        email = ""
    }
    let expectedValidationError: List<ValidationItem> = [
        { 
            message = "Must not be empty"
            property = "email"
            errorCode = "isNotEmpty" 
        }
    ]
    let authorController = new AuthorController(LoggerStub)


    let result = authorController.Post(createAuthorDto) :?> BadRequestObjectResult

    Assert.Equal(400, result.StatusCode.Value)
    Assert.Equal<List<ValidationItem>>(expectedValidationError, result.Value |> unbox)
