import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, ResponseType } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from './../app.api'
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Options } from 'selenium-webdriver/safari';

@Injectable()
export class HeaaderService {
    public token: string;
    public email: string;
    public response;

    constructor(protected httpClient: HttpClient, private http: Http) {}

    UsuariosClans(): Observable<any> {
      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));

      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('No-Auth', 'true');
      headers.append('Authorization', `Bearer ${currentUser.token}`);
      headers.append('x-access-token', `${currentUser.token}`);


      let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`)
      ;

      return this.httpClient.get<any>(`${API_URL}/api/UsuariosClans/${currentUser.email}`,
        {
          headers: httpHeaders,
          responseType: 'json'
        });

    }
}
