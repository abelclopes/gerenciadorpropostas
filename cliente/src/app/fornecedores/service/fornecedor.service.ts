import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, RequestMethod, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { API_URL } from './../../app.api';
import { Fornecedor, FornecedorModel } from '../../logica-api';
import { PagedListModel } from '../../shared/paginacao/PagedListModel';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

@Injectable()
export class FornecedorService {

    httpHeaders: HttpHeaders;

    constructor(protected httpClient: HttpClient, private http: Http) {

      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
       this.httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
    }

    public getFornecedors(pagina, tamanho, buscaTermo?: any): Observable<PagedListModel> {
      if(buscaTermo == undefined){ buscaTermo ='';}
      return this.httpClient.get<PagedListModel>(`${API_URL}/api/fornecedores?PageNumber=${pagina}&PageSize=${tamanho}&buscaTermo=${buscaTermo}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public novaFornecedor(model: FornecedorModel){
      let data = JSON.stringify(model);
      return this.httpClient.post<Fornecedor>(`${API_URL}/api/fornecedores/`,data,{headers: this.httpHeaders, responseType: 'json'});
    }
    public getFornecedorById(id: string): Observable<FornecedorModel> {
      return this.httpClient.get<FornecedorModel>(`${API_URL}/api/fornecedores/${id}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public updateFornecedor(model: Fornecedor): Observable<PagedListModel> {
      let data = JSON.stringify(model);
      let url = `${API_URL}/api/fornecedores/${model.id}`;
      return this.httpClient.put(url,data,
        {
          headers: this.httpHeaders
        }
      );
    }
    public delete(id: string): any {
      let url = `${API_URL}/api/fornecedores/${id}`;
      return this.httpClient.delete(url, { headers: this.httpHeaders } );
    }
}
