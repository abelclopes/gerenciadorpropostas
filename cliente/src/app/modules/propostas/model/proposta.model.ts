import { Categoria, Fornecedor, PropostaHistorico } from "../../../logica-api";


export class PropostaModel {
  constructor(
    public nomeProposta?: string,
    public descricao?: string,
    public valor?: number,
    public fornecedor?: Fornecedor,
    public categoria?: Categoria,
    public status?: Proposta.StatusEnum,
    public anexo?: string,
    public id?: string,
    public dataAtualizacao?: Date,
    public dataCriacao?: Date,
    public excluido?: boolean
  ){}
}
export namespace Proposta {
    export type StatusEnum = 1 | 2 | 3;
    export const StatusEnum = {
        NUMBER_1: 1 as StatusEnum,
        NUMBER_2: 2 as StatusEnum,
        NUMBER_3: 3 as StatusEnum
    }
}
