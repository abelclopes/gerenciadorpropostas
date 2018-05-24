import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, RequestMethod, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from '../../../app.api';
import { UsuarioPagedListModel, UsuarioNovoModel } from '../model';
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
    //.set('x-access-token', `${currentUser.token}`)
    .set('www-authenticate', `${currentUser.token}`);
  }

  public getUsuarios(pagina, tamanho, buscaTermo?: any): Observable<UsuarioPagedListModel> {
    if(buscaTermo == undefined){ buscaTermo ='';}
    return this.httpClient.get<UsuarioPagedListModel>(`${API_URL}/api/usuarios?PageNumber=${pagina}&PageSize=${tamanho}&buscaTermo=${buscaTermo}`,{
      headers: this.httpHeaders, responseType: 'json'
    });
  }
  public novoUsuario(model: UsuariosModel){
    let data = JSON.stringify(model);
    return this.httpClient.post<UsuariosModel>(`${API_URL}/api/usuarios/`,data,{headers: this.httpHeaders, responseType: 'json'});
  }
  public getUsuarioById(id: string): Observable<UsuariosModel> {
    return this.httpClient.get<UsuariosModel>(`${API_URL}/api/usuarios/${id}`,{
      headers: this.httpHeaders, responseType: 'json'
    });
  }
  public updateUsuario(model: UsuarioNovoModel, id: string): Observable<UsuarioNovoModel> {
    model.id = id;
    let url = `${API_URL}/api/usuarios/${model.id}/`;
    const params = new HttpParams().set('id', model.id);  
    let newDate = model.dataNacimento;
    console.log(newDate);
    var body = {  
                nome:model.nome,cpf:model.cpf,Email:model.email,ID:model.id,dataNacimento:model.dataNacimento,perfilUsuario:model.perfilUsuario
             };  
    
    return this.httpClient.put<UsuarioNovoModel>(url, body, { headers: this.httpHeaders})
      .map(res => res)
      .catch(this.handleError);
  }
  public delete(id: string): any {
    let url = `${API_URL}/api/usuarios/${id}/`;
    return this.httpClient.delete(url, { headers: this.httpHeaders } );
  }
  
  public getPerfis(): any {
    let url = `${API_URL}/api/Usuarios/Permissoes`;
    return this.httpClient.get(url, { headers: this.httpHeaders } );
  }
  private extractData(res: Response) {
    let body = res.json();
    return body || {};
  }

  private handleError(error: any) {
      let errMsg = (error.message) ? error.message :
          error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      console.error(errMsg);
      return Observable.throw(errMsg);
  }
}
