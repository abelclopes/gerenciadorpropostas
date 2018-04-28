
/* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { Http, Headers, URLSearchParams }                    from '@angular/http';
import { RequestMethod, RequestOptions, RequestOptionsArgs } from '@angular/http';
import { Response, ResponseContentType }                     from '@angular/http';

import { Observable }                                        from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import * as models                                           from '../model/models';
import {API as BASE_PATH} from '../../app.api';
import { AuthService } from '../../auth/auth.service';

@Injectable()
export class PropostasApi {

    protected basePath;
    public defaultHeaders: Headers = new Headers();
    Url: string
    constructor(protected http: Http, @Optional()@Inject(BASE_PATH) basePath: string) {
        if (basePath) {
            this.Url = basePath;
        }
        this.Url = `${BASE_PATH}`;
    }

    /**
     *
     * @param propostaId
     * @param id
     */
    public apiPropostasBypropostaIdDelete(propostaId: string, extraHttpRequestParams?: any): Observable<models.PropostasModel> {
        return this.apiPropostasBypropostaIdDeleteWithHttpInfo(propostaId, extraHttpRequestParams)
            .map((response: Response) => {
                if (response.status === 204) {
                    return undefined;
                } else {
                    return response.json() || {};
                }
            });
    }

    /**
     *
     * @param propostaId
     */
    public apiPropostasBypropostaIdGet(propostaId: string, extraHttpRequestParams?: any): Observable<models.PropostasModel> {
        return this.apiPropostasBypropostaIdGetWithHttpInfo(propostaId, extraHttpRequestParams)
            .map((response: Response) => {
                if (response.status === 204) {
                    return undefined;
                } else {
                    return response.json() || {};
                }
            });
    }

    /**
     *
     * @param propostaId
     * @param propostas
     */
    public apiPropostasBypropostaIdPut(proposta_id: string, proposta: models.PropostasModel, extraHttpRequestParams?: any): Observable<models.PropostasModel> {
        return this.apiPropostasBypropostaIdPutWithHttpInfo(proposta_id, proposta, extraHttpRequestParams)
            .map((response: Response) => {
                if (response.status === 204) {
                    return undefined;
                } else {
                    return response.json() || {};
                }
            });
    }

    /**
     *
     * @param numeroPagina
     * @param tamanhoPagina
     */
    public apiPropostasGet(numeroPagina?: number, tamanhoPagina?: number, extraHttpRequestParams?: any): Observable<models.ListaPaginadaPropostasModel> {
        return this.apiPropostasGetWithHttpInfo(numeroPagina, tamanhoPagina, extraHttpRequestParams)
            .map((response: Response) => {
                if (response.status === 204) {
                    return undefined;
                } else {
                    return response.json() || {};
                }
            });
    }

    /**
     *
     * @param propostas
     */
    public apiPropostasPost(proposta?: models.PropostasModel, extraHttpRequestParams?: any): Observable<models.PropostasModel> {
        return this.apiPropostasPostWithHttpInfo(proposta, extraHttpRequestParams)
            .map((response: Response) => {
                if (response.status === 204) {
                    return undefined;
                } else {
                    return response.json() || {};
                }
            });
    }


    /**
     *
     *
     * @param propostaId
     * @param id
     */
    public apiPropostasBypropostaIdDeleteWithHttpInfo(propostaId: string, id?: string, extraHttpRequestParams?: any): Observable<Response> {
        const path = this.Url + '/Propostas/${propostaId}'
                    .replace('${' + 'propostaId' + '}', String(propostaId));

        let queryParameters = new URLSearchParams();
        let headers = new Headers(this.defaultHeaders.toJSON()); // https://github.com/angular/angular/issues/6845
        // verify required parameter 'propostaId' is not null or undefined
        if (propostaId === null || propostaId === undefined) {
            throw new Error('Required parameter propostaId was null or undefined when calling apiPropostasBypropostaIdDelete.');
        }
        if (id !== undefined) {
            queryParameters.set('id', <any>id);
        }

        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        // to determine the Accept header
        let produces: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];

        let requestOptions: RequestOptionsArgs = new RequestOptions({
            method: RequestMethod.Delete,
            headers: headers,
            search: queryParameters,
            withCredentials: AuthService.isAuthenticated()
        });
        if (extraHttpRequestParams) {
            requestOptions = (<any>Object).assign(requestOptions, extraHttpRequestParams);
        }

        return this.http.request(path, requestOptions);
    }

    /**
     *
     *
     * @param propostaId
     */
    public apiPropostasBypropostaIdGetWithHttpInfo(propostaId: string, extraHttpRequestParams?: any): Observable<Response> {
        const path = this.Url + '/Propostas/${propostaId}'
                    .replace('${' + 'propostaId' + '}', String(propostaId));

        let queryParameters = new URLSearchParams();
        let headers = new Headers(this.defaultHeaders.toJSON());
        // verify required parameter 'propostaId' is not null or undefined
        if (propostaId === null || propostaId === undefined) {
            throw new Error('Required parameter propostaId was null or undefined when calling apiPropostasBypropostaIdGet.');
        }
        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        // to determine the Accept header
        let produces: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];

        let requestOptions: RequestOptionsArgs = new RequestOptions({
            method: RequestMethod.Get,
            headers: headers,
            search: queryParameters,
            withCredentials: AuthService.isAuthenticated()
        });

        if (extraHttpRequestParams) {
            requestOptions = (<any>Object).assign(requestOptions, extraHttpRequestParams);
        }

        return this.http.request(path, requestOptions);
    }

    /**
     *
     *
     * @param propostaId
     * @param propostas
     */
    public apiPropostasBypropostaIdPutWithHttpInfo(propostaId: string, proposta: models.PropostasModel, extraHttpRequestParams?: any): Observable<Response> {
        const path = this.Url + '/Propostas/${propostaId}'
                    .replace('${' + 'propostaId' + '}', String(propostaId));
        let queryParameters = new URLSearchParams();
      //  let headers = new Headers(this.defaultHeaders.toJSON()); // https://github.com/angular/angular/issues/6845
        // verify required parameter 'propostaId' is not null or undefined
        if (propostaId === null || propostaId === undefined)
           throw new Error('Required parameter propostaId was null or undefined when calling apiGruposBypropostaIdPut.');

           let headers = new Headers(
           {
               'Content-Type': 'application/json'
           });

        let requestOptions: RequestOptionsArgs = new RequestOptions({
           method: RequestMethod.Put,
           headers: headers,
           body: JSON.stringify(proposta)
        });
        // https://github.com/swagger-api/swagger-codegen/issues/4037
        if (extraHttpRequestParams) {
           requestOptions = (<any>Object).assign(requestOptions, extraHttpRequestParams);
        }

        return this.http.request(path, requestOptions);
    }

    /**
     *
     *
     * @param numeroPagina
     * @param tamanhoPagina
     */
    public apiPropostasGetWithHttpInfo(numeroPagina?: number, tamanhoPagina?: number, extraHttpRequestParams?: any): Observable<Response> {
        const path = this.Url + `/Propostas/${numeroPagina}/${tamanhoPagina}`;

        let queryParameters = new URLSearchParams();
        let headers = new Headers(this.defaultHeaders.toJSON());


        // to determine the Content-Type header
        let consumes: string[] = [
        ];

        // to determine the Accept header
        let produces: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];

        let requestOptions: RequestOptionsArgs = new RequestOptions({
            method: RequestMethod.Get,
            headers: headers,
            search: queryParameters
        });

        if (extraHttpRequestParams) {
            requestOptions = (<any>Object).assign(requestOptions, extraHttpRequestParams);
        }

        return this.http.request(path, requestOptions);
    }

    /**
     *
     *
     * @param propostas
     */
    public apiPropostasPostWithHttpInfo(proposta?: models.PropostasModel, extraHttpRequestParams?: any): Observable<Response> {
        const path = this.Url + '/Propostas';

        let queryParameters = new URLSearchParams();
        let headers = new Headers(
        {
            'Content-Type': 'application/json'
        });


        let requestOptions: RequestOptionsArgs = new RequestOptions({
            method: RequestMethod.Post,
            headers: headers,
            body: JSON.stringify(proposta)
        });

        return this.http.request(path, requestOptions);
    }

}
