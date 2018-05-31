import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from './../../../app.api'
import { UsuariosClans } from '../../../modules/usuarios/model/usuario-clans.model';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { tokenUser } from '../model/token-user';

@Injectable()
export class AuthenticationService {
    public token: string;

    constructor(private http: Http,private httpClient: HttpClient) {
        // set token if saved in local storage
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
    }

    login(email: string, password: string): Observable<boolean> {
        return this.http.post(API_URL +'/api/auth', { 'email': email, 'password': password })
            .map((response: Response) => {
                  let token = response.json() && response.json().token;
                if (token) {
                    this.token = token;
                    localStorage.setItem('usuarioCorrente', JSON.stringify({ email: email, token: token }));
                    return true;
                } else {
                    return false;
                }
            });
    }
    logout(): void {
        this.token = null;
        localStorage.removeItem('usuarioCorrente');
    }
  UsuariosClans(): Observable<UsuariosClans> {

    const currentUser: tokenUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
    if(currentUser != null){
      let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', currentUser.token);
      return this.httpClient.get<UsuariosClans>(`${API_URL}/api/Usuarios/Clans/${currentUser.email}`,
      {
        headers: httpHeaders,
        responseType: 'json'
      });
    }
  }
}
