import { UsuariosModel } from ".";

export interface UsuarioPagedListModel {
    totalItens?: number;
    pageNumber?: number;
    pageSize?: number;
    resultado?: Array<UsuariosModel>;
    totalPages?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
    nextPageNumber?: number;
    previousPageNumber?: number;
}
