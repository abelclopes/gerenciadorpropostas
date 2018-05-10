import { PropostaModel } from "../../../logica-api";

export interface PropostaPagedListModel {
    totalItens?: number;
    pageNumber?: number;
    pageSize?: number;
    resultado?: Array<PropostaModel>;
    totalPages?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
    nextPageNumber?: number;
    previousPageNumber?: number;
}
