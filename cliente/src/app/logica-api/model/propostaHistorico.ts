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
import { Proposta } from './proposta';
import { Usuario } from './usuario';


export interface PropostaHistorico {
    propostaId?: string;
    proposta?: Proposta;
    propostaStatus?: PropostaHistorico.PropostaStatusEnum;
    usuarioId?: string;
    usuario?: Usuario;
    id?: string;
    dataCriacao?: Date;
    dataAtualizacao?: Date;
    excluido?: boolean;
}
export namespace PropostaHistorico {
    export type PropostaStatusEnum = 1 | 2 | 3 | 4;
    export const PropostaStatusEnum = {
        NUMBER_1: 1 as PropostaStatusEnum,
        NUMBER_2: 2 as PropostaStatusEnum,
        NUMBER_3: 3 as PropostaStatusEnum,
        NUMBER_4: 4 as PropostaStatusEnum
    }
}
