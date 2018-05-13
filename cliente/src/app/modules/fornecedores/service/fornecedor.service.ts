import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, RequestMethod, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { API_URL } from '../../../app.api';
import { FornecedorPagedListModel, FornecedorModel } from '../model';

@Injectable()
export class FornecedorService {

    httpHeaders: HttpHeaders;

    constructor(protected httpClient: HttpClient, private http: Http) {

      const currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
       this.httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
    }

    public getFornecedors(pagina, tamanho, buscaTermo?: any): Observable<FornecedorPagedListModel> {
      const url = `${API_URL}/api/fornecedores?PageNumber=${pagina}&PageSize=${tamanho}&buscaTermo=${buscaTermo}`;
      return this.httpClient.get<FornecedorPagedListModel>(url, {
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public novaFornecedor(model: FornecedorModel) {
      const data = JSON.stringify(model);
      return this.httpClient.post<FornecedorModel>(`${API_URL}/api/fornecedores/`, data, {headers: this.httpHeaders, responseType: 'json'});
    }
    public getFornecedorById(id: string): Observable<FornecedorModel> {
      return this.httpClient.get<FornecedorModel>(`${API_URL}/api/fornecedores/${id}`, {
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public updateFornecedor(model: FornecedorModel): Observable<FornecedorPagedListModel> {
      const data = JSON.stringify(model);
      const url = `${API_URL}/api/fornecedores/${model.id}`;
      return this.httpClient.put(url, data,
        {
          headers: this.httpHeaders
        }
      );
    }
    public delete(id: string): any {
      const url = `${API_URL}/api/fornecedores/${id}`;
      return this.httpClient.delete(url, { headers: this.httpHeaders } );
    }
    
    search(busca: any){
      const url = `${API_URL}/api/fornecedores/getall/${busca}`;
      return this.httpClient.get(url, { headers: this.httpHeaders } );
    }
}
