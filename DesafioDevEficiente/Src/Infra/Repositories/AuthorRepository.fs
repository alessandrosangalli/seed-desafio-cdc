namespace DesafioDevEficiente.Infra.Repositories

open System
open MySql.Data.MySqlClient
open Dapper

module AuthorModels =
    type QueryParameters = { Email: string }

    type CreateAuthor = {
        Name: string
        Email: string
        Description: string
    }

    type Author = {
        Id: int32
        Name: string
        Email: string
        Description: string
        CreatedAt: DateTime
    }

type IAuthorRepository =
    abstract member Add: AuthorModels.CreateAuthor -> int
    abstract member FindByEmail: string -> option<AuthorModels.Author>

type AuthorRepository(connectionString: string) =
    interface IAuthorRepository with
        member _.Add(author: AuthorModels.CreateAuthor) =
            use connection = new MySqlConnection(connectionString)
            connection.Open()

            let query = """
                INSERT INTO Authors (Name, Email, Description)
                VALUES (@Name, @Email, @Description)
            """
            connection.Execute(query, author)

        member _.FindByEmail(email: string): option<AuthorModels.Author> =
            use connection = new MySqlConnection(connectionString)
            connection.Open()

            let query = "SELECT Id, Name, Email, Description, CreatedAt FROM Authors WHERE Email = @Email"
            let parameters = { AuthorModels.QueryParameters.Email = email }

            connection.Query<AuthorModels.Author>(query, parameters) |> Seq.tryHead