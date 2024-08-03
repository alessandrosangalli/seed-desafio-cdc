module DesafioDevEficiente.ControllerDtos

open System

type CreateAuthorDto = {
    name: string
    description: string
    email: string
}