namespace DesafioDevEficiente.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open AccidentalFish.FSharp.Validation
open DesafioDevEficiente.UseCases
open DesafioDevEficiente.UseCases.Error

open Validators

[<ApiController>]
[<Route("[controller]")>]
type AuthorController (logger : ILogger<AuthorController>, createAuthorUseCase : ICreateAuthorUseCase) =
    inherit ControllerBase()

    [<HttpPost>]
    member this.Post([<FromBody>] createAuthorControllerInput) =
        match validateAuthor createAuthorControllerInput with
        | Ok -> 
            match createAuthorUseCase.Execute(this.ControllerInputToUseCase(createAuthorControllerInput)) with
            | Right _ -> this.Ok() :> IActionResult
            | Left error -> 
                match error with
                | DuplicatedEmail _ -> this.Conflict(CreateAuthorErrorUtil.getErrorString(error)) :> IActionResult
                | InternalError _ -> this.Problem() :> IActionResult
        | Errors errors -> this.BadRequest(errors) :> IActionResult
        
    member private this.ControllerInputToUseCase(createAuthorControllerInput)  =
         {  Name = createAuthorControllerInput.name; Description = createAuthorControllerInput.description; Email = createAuthorControllerInput.email}
        