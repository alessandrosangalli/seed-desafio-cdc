module AuthorControllerTests

open System
open Xunit
open DesafioDevEficiente.Controllers
open DesafioDevEficiente.ControllerDtos
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
let ``Valid dto`` () =
    let createAuthorDto = {
        instant = new DateTime()
        email = "valid@mail.com"
        name = "John Doe"
        description = "My description"
    }

    let authorController = new AuthorController(LoggerStub)

    let result = authorController.Post(createAuthorDto) :?> OkResult

    Assert.Equal(200, result.StatusCode)

[<Fact>]
let ``Email can't be empty`` () =
    let createAuthorDto = {
        instant = new DateTime()
        email = ""
        name = "John Doe"
        description = "My description"
    }
    let expectedValidationError: List<ValidationItem> = [
        {
            errorCode = "Must be a valid email"
            message = "email"
            property = "isNotValidFormat"
        }
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

[<Fact>]
let ``Email must be in a valid format`` () =
    let createAuthorDto = {
        instant = new DateTime()
        email = "@invalid.com@mail"
        name = "John Doe"
        description = "My description"
    }
    let expectedValidationError: List<ValidationItem> = [
        {
            errorCode = "Must be a valid email"
            message = "email"
            property = "isNotValidFormat"
        }
    ]
    let authorController = new AuthorController(LoggerStub)

    let result = authorController.Post(createAuthorDto) :?> BadRequestObjectResult

    Assert.Equal(400, result.StatusCode.Value)
    Assert.Equal<List<ValidationItem>>(expectedValidationError, result.Value |> unbox)

[<Fact>]
let ``Name can't be empty`` () =
    let createAuthorDto = {
        instant = new DateTime()
        email = "valid@mail.com"
        name = ""
        description = "My description"
    }
    let expectedValidationError: List<ValidationItem> = [
        { 
            message = "Must not be empty"
            property = "name"
            errorCode = "isNotEmpty" 
        }
    ]
    let authorController = new AuthorController(LoggerStub)

    let result = authorController.Post(createAuthorDto) :?> BadRequestObjectResult

    Assert.Equal(400, result.StatusCode.Value)
    Assert.Equal<List<ValidationItem>>(expectedValidationError, result.Value |> unbox)

[<Fact>]
let ``Description can't be empty`` () =
    let createAuthorDto = {
        instant = new DateTime()
        email = "valid@mail.com"
        name = "John Doe"
        description = ""
    }
    let expectedValidationError: List<ValidationItem> = [
        { 
            message = "Must not be empty"
            property = "description"
            errorCode = "isNotEmpty" 
        }
    ]
    let authorController = new AuthorController(LoggerStub)

    let result = authorController.Post(createAuthorDto) :?> BadRequestObjectResult

    Assert.Equal(400, result.StatusCode.Value)
    Assert.Equal<List<ValidationItem>>(expectedValidationError, result.Value |> unbox)

[<Fact>]
let ``Description can't be greater than 400`` () =
    let createAuthorDto = {
        instant = new DateTime()
        email = "valid@mail.com"
        name = "John Doe"
        description = "401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 chars description 401 c"
    }
    let expectedValidationError: List<ValidationItem> = [
        { 
            message = "Must have a maximum value of 400"
            property = "description.Length"
            errorCode = "isLessThanOrEqualTo" 
        }
    ]
    let authorController = new AuthorController(LoggerStub)

    let result = authorController.Post(createAuthorDto) :?> BadRequestObjectResult

    Assert.Equal(400, result.StatusCode.Value)
    Assert.Equal<List<ValidationItem>>(expectedValidationError, result.Value |> unbox)