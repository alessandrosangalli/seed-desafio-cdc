namespace DesafioDevEficiente.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open AccidentalFish.FSharp.Validation
open DesafioDevEficiente

type CreateAuthorDto = {
    email: string
}

[<ApiController>]
[<Route("[controller]")>]
type AuthorController (logger : ILogger<AuthorController>) as this =
    inherit ControllerBase()

    [<HttpPost>]
    member _.Post([<FromBody>] createAuthorDto) =
        let validateAuthor = createValidatorFor<CreateAuthorDto>() {
            validate(fun o -> o.email) [
                isNotEmpty
            ]
        }

        match validateAuthor createAuthorDto with
        | Ok -> this.Ok() :> IActionResult
        | Errors errors -> this.BadRequest(errors) :> IActionResult
        
