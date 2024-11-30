namespace DesafioDevEficiente.UseCases.Error

type CreateAuthorError = 
    | DuplicatedEmail of string 
    | InternalError of string

module CreateAuthorErrorUtil =
    let duplicatedEmail =
        DuplicatedEmail "This email is already registered."
    let internalError =
        InternalError "Internal error."

    let getErrorString(error: CreateAuthorError) =
        function
        | DuplicatedEmail message -> message
        | InternalError message -> message
        