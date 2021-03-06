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


export interface Fornecedor {
    nome?: string;
    cnpjCpf?: string;
    email?: string;
    telefone?: string;
    propostas?: Array<Proposta>;
    id?: string;
    dataCriacao?: Date;
    dataAtualizacao?: Date;
    excluido?: boolean;
}
