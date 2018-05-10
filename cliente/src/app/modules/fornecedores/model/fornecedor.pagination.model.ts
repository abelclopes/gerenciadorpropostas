import { FornecedorModel } from "../../../logica-api";

export interface FornecedorPagedListModel {
    totalItens?: number;
    pageNumber?: number;
    pageSize?: number;
    resultado?: Array<FornecedorModel>;
    totalPages?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
    nextPageNumber?: number;
    previousPageNumber?: number;
}
