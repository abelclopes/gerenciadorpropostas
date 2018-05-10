
export class UsuarioNovoModel {
  constructor(
  public nome?: string,
  public email?: string,
  public cpf?: string,
  public perfil?: UsuarioNovoModel.PerfilEnum,
  public dataNacimento?: Date,
  public senha?: string
){}
}
export namespace UsuarioNovoModel {
  export type PerfilEnum = 1 | 2 | 3 | 4;
  export const PerfilEnum = {
      NUMBER_1: 1 as PerfilEnum,
      NUMBER_2: 2 as PerfilEnum,
      NUMBER_3: 3 as PerfilEnum,
      NUMBER_4: 4 as PerfilEnum
  }
}
