module Validators

open AccidentalFish.FSharp.Validation
open DesafioDevEficiente.ControllerDtos
open System.Text.RegularExpressions

let validateAuthor = createValidatorFor<CreateAuthorDto>() {
    validate(fun o -> o.email) [
        isNotEmpty
    ]
    validate(fun o -> o.name) [
        isNotEmpty
    ]
    validate(fun o -> o.description) [
        isNotEmpty
    ]
    validate(fun o -> o.description.Length) [
        isLessThanOrEqualTo 400
    ]
    validate (fun o -> o) [
        withFunction (fun o ->
            match Regex.IsMatch(o.email, @"[^@]+@[^\.]+\..+") with
            | true -> Ok
            | false -> Errors([
                {
                    errorCode="Must be a valid email"
                    message="email"
                    property = "isNotValidFormat"
                }
            ])
        )
    ]
}