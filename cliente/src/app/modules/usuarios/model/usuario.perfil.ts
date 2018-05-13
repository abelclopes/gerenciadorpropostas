export type PerfilUsuarioEnum = 1 | 2 | 3 | 4;
export const PerfilUsuario = {
    'Administrador': 1 as PerfilUsuarioEnum,
    'Analista de comprar': 2 as PerfilUsuarioEnum,
    'Analista Financeiro': 3 as PerfilUsuarioEnum,
    'Diretor Financeiro': 4 as PerfilUsuarioEnum
}
export class Perfil{
    constructor(public id:number, public descricao: string){}
}