import { Categoria, Fornecedor } from "../../../logica-api";


export class PropostaNovaModel {
  constructor(
    public nomeProposta?: string,
    public descricao?: string,
    public valor?: number,
    public fornecedor?: Fornecedor,
    public categoria?: Categoria,
    public status?: PropostaNovaModel.StatusEnum,
    public fornecedorID?: string,
    public categoriaID?: string,
    public anexo?: string,
    public files?: File,
    public id?: string,
    public usuario?: string,
    public dataAtualizacao?: Date,
    public dataCriacao?: Date,
    public excluido?: boolean
  ){}
}
export namespace PropostaNovaModel {
    export type StatusEnum = 1 | 2 | 3;
    export const StatusEnum = {
        NUMBER_1: 1 as StatusEnum,
        NUMBER_2: 2 as StatusEnum,
        NUMBER_3: 3 as StatusEnum
    }
}
