import { CategoriaModel } from "./categoria.model";

export interface CategoriaPagedListModel {
    totalItens?: number;
    pageNumber?: number;
    pageSize?: number;
    resultado?: Array<CategoriaModel>;
    totalPages?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
    nextPageNumber?: number;
    previousPageNumber?: number;
}
