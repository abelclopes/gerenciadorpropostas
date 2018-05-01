import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from './../app.api'
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { usuarioAuthModel } from '../logica-api';

@Injectable()
export class HeaaderService {
      public token: string;
      public email: string;

    constructor(protected httpClient: HttpClient, private http: Http) {}

    UsuariosClans() {
        var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
        this.token = currentUser && currentUser.token;
        this.email = currentUser.email;
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type':  'application/json',
            'Authorization': 'bearer ' +  currentUser.token,
          })
        };
        return this.httpClient.get<any>(`${API_URL}/api/UsuariosClans${this.email}`, httpOptions)
          .map((res)=>{
            console.log(res);
            return false;
          });
    }
  teste(){
    var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
    if(currentUser != null){
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('Authorization', `bearer ${currentUser.token}`);
      // let obs = new Observable(observer => {
          this.http.get(`${API_URL}/api/UsuariosClans${currentUser.email}`, {headers: headers, body:'' }).subscribe(
              (response: Response) => {

                console.log(response);
              },
              error=> {
                console.log(error);
              });
    }
    // });
  }
}
