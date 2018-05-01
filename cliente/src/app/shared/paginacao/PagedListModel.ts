export interface PagedListModel {
    totalItems?: number;
    pageNumber?: number;
    pageSize?: number;
    resultado?: Array<any>;
    totalPages?: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
    nextPageNumber?: number;
    previousPageNumber?: number;
}
