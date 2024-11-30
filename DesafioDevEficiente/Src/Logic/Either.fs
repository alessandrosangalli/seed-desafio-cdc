[<AutoOpen>]
module Either

type Either<'L, 'R> =
    | Left of 'L
    | Right of 'R

module Either =
    let map f = function
        | Right r -> Right (f r)
        | Left l -> Left l

    let bind f = function
        | Right r -> f r
        | Left l -> Left l

    let isLeft = function
        | Left _ -> true
        | Right _ -> false

    let isRight = function
        | Right _ -> true
        | Left _ -> false

    let fromOption opt defaultValue =
        match opt with
        | Some value -> Right value
        | None -> Left defaultValue

    let toOption = function
        | Right r -> Some r
        | Left _ -> None