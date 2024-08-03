namespace DesafioDevEficiente.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open AccidentalFish.FSharp.Validation
open Validators

[<ApiController>]
[<Route("[controller]")>]
type AuthorController (logger : ILogger<AuthorController>) as this =
    inherit ControllerBase()

    [<HttpPost>]
    member _.Post([<FromBody>] createAuthorDto) =
        match validateAuthor createAuthorDto with
        | Ok -> this.Ok() :> IActionResult
        | Errors errors -> this.BadRequest(errors) :> IActionResult
        
