import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, ResponseType } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from './../../app.api'
import { HttpHeaders, HttpClient } from '@angular/common/http';

@Injectable()
export class HeaderService {
    public token: string;
    public email: string;
    public response;
//    protected httpClient: HttpClient; private http: Http;
    constructor(protected httpClient: HttpClient, private http: Http) {}

    UsuariosClans(): Observable<any> {

      const currentUser: tokenUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
      let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', currentUser.token);
      return this.httpClient.get(`${API_URL}/api/UsuariosClans/${currentUser.email}`,
        {
          headers: httpHeaders,
          responseType: 'json'
        });
    }
}
export class tokenUser{
  constructor(public token?:string, public email?: string){}
}
