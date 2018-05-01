/**
 * Gerenciador de Propostas API
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */


export interface NovoUsuarioModel {
    nome?: string;
    cpf?: string;
    email?: string;
    senha?: string;
    dataNacimento?: Date;
    perfilUsuario?: NovoUsuarioModel.PerfilUsuarioEnum;
}
export namespace NovoUsuarioModel {
    export type PerfilUsuarioEnum = 1 | 2 | 3 | 4;
    export const PerfilUsuarioEnum = {
        NUMBER_1: 1 as PerfilUsuarioEnum,
        NUMBER_2: 2 as PerfilUsuarioEnum,
        NUMBER_3: 3 as PerfilUsuarioEnum,
        NUMBER_4: 4 as PerfilUsuarioEnum
    }
}
