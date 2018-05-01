import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from './../../app.api';
import { Categoria } from '../../logica-api';

@Injectable()
export class CategoriasService {
    httpHeaders: HttpHeaders;
    constructor(protected httpClient: HttpClient, private http: Http) {

      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
       this.httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
    }

    public getCategorias(): Observable<any> {
      return this.httpClient.get<any>(`${API_URL}/api/categorias/`,{headers: this.httpHeaders, responseType: 'json'});
    }
    public novaCategoria(categoria: Categoria){
      return this.httpClient.post<Categoria>(`${API_URL}/api/categorias/`,{categoria: categoria},{headers: this.httpHeaders, responseType: 'json'});
    }
}
