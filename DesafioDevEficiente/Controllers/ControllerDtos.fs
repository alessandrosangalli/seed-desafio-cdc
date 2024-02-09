module DesafioDevEficiente.ControllerDtos

open System

type CreateAuthorDto = {
    instant: DateTime
    email: string
    name: string
    description: string
}