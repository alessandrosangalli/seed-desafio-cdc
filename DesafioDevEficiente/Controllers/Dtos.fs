module DesafioDevEficiente.Dtos

open System

type CreateAuthorDto = {
    instant: DateTime
    email: string
    name: string
    description: string
}