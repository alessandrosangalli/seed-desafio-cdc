namespace DesafioDevEficiente.UseCases

open DesafioDevEficiente.Infra.Repositories
open DesafioDevEficiente.UseCases.Error

module CreateAuthorUseCaseModels =
    type Input = {
        Name: string
        Description: string
        Email: string
    }

    type Output = {
        Description: string
}

type ICreateAuthorUseCase =
    abstract member Execute: CreateAuthorUseCaseModels.Input -> Either<CreateAuthorError, unit>

type CreateAuthorUseCase(repository: IAuthorRepository) as this =
    interface ICreateAuthorUseCase with
        member _.Execute(input: CreateAuthorUseCaseModels.Input): Either<CreateAuthorError, unit> =
            if this.isEmailDuplicated input.Email then
                Left CreateAuthorErrorUtil.duplicatedEmail
            else
                match repository.Add({ Name = input.Name; Email = input.Email; Description = input.Description }) with
                | 1 -> Right()
                | _ -> Left CreateAuthorErrorUtil.internalError

    member private _.isEmailDuplicated(email: string): bool =
        Option.isSome(repository.FindByEmail(email))