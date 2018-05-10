
export class UsuariosModel {
  constructor(
  public nome?: string,
  public email?: string,
  public cpf?: string,
  public perfil?: UsuariosModel.PerfilEnum,
  public dataNacimento?: Date,
  public id?: string,
  public dataAtualizacao?: Date,
  public dataCriacao?: Date,
  public excluido?: boolean
){}
}
export namespace UsuariosModel {
  export type PerfilEnum = 1 | 2 | 3 | 4;
  export const PerfilEnum = {
      NUMBER_1: 1 as PerfilEnum,
      NUMBER_2: 2 as PerfilEnum,
      NUMBER_3: 3 as PerfilEnum,
      NUMBER_4: 4 as PerfilEnum
  }
}
