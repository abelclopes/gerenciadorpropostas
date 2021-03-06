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
/* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs/Observable';

import { NovaPropostaModel } from '../model/novaPropostaModel';
import { PagedListProposta } from '../model/pagedListProposta';
import { PropostaModel } from '../model/propostaModel';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class PropostasService {

    protected basePath = '';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (let consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * 
     * 
     * @param id 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiPropostasByIdDelete(id: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiPropostasByIdDelete(id: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiPropostasByIdDelete(id: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiPropostasByIdDelete(id: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {
        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiPropostasByIdDelete.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        let httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set("Accept", httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        return this.httpClient.delete<any>(`${this.basePath}/api/Propostas/${encodeURIComponent(String(id))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * 
     * 
     * @param id 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiPropostasByIdGet(id: string, observe?: 'body', reportProgress?: boolean): Observable<PropostaModel>;
    public apiPropostasByIdGet(id: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PropostaModel>>;
    public apiPropostasByIdGet(id: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PropostaModel>>;
    public apiPropostasByIdGet(id: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {
        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiPropostasByIdGet.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        let httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set("Accept", httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        return this.httpClient.get<PropostaModel>(`${this.basePath}/api/Propostas/${encodeURIComponent(String(id))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * 
     * 
     * @param id 
     * @param model 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiPropostasByIdPut(id: string, model?: NovaPropostaModel, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiPropostasByIdPut(id: string, model?: NovaPropostaModel, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiPropostasByIdPut(id: string, model?: NovaPropostaModel, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiPropostasByIdPut(id: string, model?: NovaPropostaModel, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {
        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiPropostasByIdPut.');
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        let httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set("Accept", httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        let consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        let httpContentTypeSelected:string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set("Content-Type", httpContentTypeSelected);
        }

        return this.httpClient.put<any>(`${this.basePath}/api/Propostas/${encodeURIComponent(String(id))}`,
            model,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * 
     * 
     * @param pageNumber 
     * @param pageSize 
     * @param nomeProposta 
     * @param descricao 
     * @param valor 
     * @param fornecedorID 
     * @param categoriaID 
     * @param status 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiPropostasGet(pageNumber?: number, pageSize?: number, nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, observe?: 'body', reportProgress?: boolean): Observable<PagedListProposta>;
    public apiPropostasGet(pageNumber?: number, pageSize?: number, nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PagedListProposta>>;
    public apiPropostasGet(pageNumber?: number, pageSize?: number, nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PagedListProposta>>;
    public apiPropostasGet(pageNumber?: number, pageSize?: number, nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (pageNumber !== undefined) {
            queryParameters = queryParameters.set('PageNumber', <any>pageNumber);
        }
        if (pageSize !== undefined) {
            queryParameters = queryParameters.set('PageSize', <any>pageSize);
        }
        if (nomeProposta !== undefined) {
            queryParameters = queryParameters.set('NomeProposta', <any>nomeProposta);
        }
        if (descricao !== undefined) {
            queryParameters = queryParameters.set('Descricao', <any>descricao);
        }
        if (valor !== undefined) {
            queryParameters = queryParameters.set('Valor', <any>valor);
        }
        if (fornecedorID !== undefined) {
            queryParameters = queryParameters.set('FornecedorID', <any>fornecedorID);
        }
        if (categoriaID !== undefined) {
            queryParameters = queryParameters.set('CategoriaID', <any>categoriaID);
        }
        if (status !== undefined) {
            queryParameters = queryParameters.set('Status', <any>status);
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        let httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set("Accept", httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        return this.httpClient.get<PagedListProposta>(`${this.basePath}/api/Propostas`,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * 
     * 
     * @param nomeProposta 
     * @param descricao 
     * @param valor 
     * @param fornecedorID 
     * @param categoriaID 
     * @param status 
     * @param usuario 
     * @param anexo 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public apiPropostasPost(nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, usuario?: string, anexo?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiPropostasPost(nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, usuario?: string, anexo?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiPropostasPost(nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, usuario?: string, anexo?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiPropostasPost(nomeProposta?: string, descricao?: string, valor?: number, fornecedorID?: string, categoriaID?: string, status?: number, usuario?: string, anexo?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (anexo !== undefined) {
            queryParameters = queryParameters.set('Anexo', <any>anexo);
        }

        let headers = this.defaultHeaders;

        // authentication (Bearer) required
        if (this.configuration.apiKeys["Authorization"]) {
            headers = headers.set('Authorization', this.configuration.apiKeys["Authorization"]);
        }

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        let httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set("Accept", httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        const canConsumeForm = this.canConsumeForm(consumes);

        let formParams: { append(param: string, value: any): void; };
        let useForm = false;
        let convertFormParamsToString = false;
        if (useForm) {
            formParams = new FormData();
        } else {
            formParams = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        }

        if (nomeProposta !== undefined) {
            formParams = formParams.append('NomeProposta', <any>nomeProposta) || formParams;
        }
        if (descricao !== undefined) {
            formParams = formParams.append('Descricao', <any>descricao) || formParams;
        }
        if (valor !== undefined) {
            formParams = formParams.append('Valor', <any>valor) || formParams;
        }
        if (fornecedorID !== undefined) {
            formParams = formParams.append('FornecedorID', <any>fornecedorID) || formParams;
        }
        if (categoriaID !== undefined) {
            formParams = formParams.append('CategoriaID', <any>categoriaID) || formParams;
        }
        if (status !== undefined) {
            formParams = formParams.append('Status', <any>status) || formParams;
        }
        if (usuario !== undefined) {
            formParams = formParams.append('Usuario', <any>usuario) || formParams;
        }

        return this.httpClient.post<any>(`${this.basePath}/api/Propostas`,
            convertFormParamsToString ? formParams.toString() : formParams,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}
