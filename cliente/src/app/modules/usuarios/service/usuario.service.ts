import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, RequestMethod, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from '../../../app.api';
import { UsuarioPagedListModel } from '../model';
import { UsuariosModel } from '../../../logica-api';

@Injectable()
export class UsuarioService {

    httpHeaders: HttpHeaders;

    constructor(protected httpClient: HttpClient, private http: Http) {

      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
       this.httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
    }

    public getUsuarios(pagina, tamanho, buscaTermo?: any): Observable<UsuarioPagedListModel> {
      if(buscaTermo == undefined){ buscaTermo ='';}
      return this.httpClient.get<UsuarioPagedListModel>(`${API_URL}/api/propostas?PageNumber=${pagina}&PageSize=${tamanho}&buscaTermo=${buscaTermo}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public novaUsuario(model: UsuariosModel){
      let data = JSON.stringify(model);
      return this.httpClient.post<UsuariosModel>(`${API_URL}/api/propostas/`,data,{headers: this.httpHeaders, responseType: 'json'});
    }
    public getUsuarioById(id: string): Observable<UsuariosModel> {
      return this.httpClient.get<UsuariosModel>(`${API_URL}/api/propostas/${id}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public updateUsuario(model: UsuariosModel): Observable<UsuarioPagedListModel> {
      let data = JSON.stringify(model);
      let url = `${API_URL}/api/propostas/${model.id}`;
      return this.httpClient.put(url,data,
        {
          headers: this.httpHeaders
        }
      );
    }
    public delete(id: string): any {
      let url = `${API_URL}/api/propostas/${id}`;
      return this.httpClient.delete(url, { headers: this.httpHeaders } );
    }
}
