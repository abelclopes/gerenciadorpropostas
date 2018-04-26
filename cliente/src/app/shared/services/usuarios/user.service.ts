import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Response } from "@angular/http";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { User } from './user.model';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class UserService {
  readonly rootUrl = 'http://localhost:5000/api';
  helper: JwtHelperService = new JwtHelperService;
  constructor(private http: HttpClient) { }

  registrarUsuario(user: User) {
    const body: User = {
      Nome: user.Nome,
      Password: user.Password,
      Email: user.Email,
      Police: user.Police,
      DataNascimento: user.DataNascimento
    }
    var reqHeader = new HttpHeaders({'No-Auth':'True'});
    return this.http.post(this.rootUrl + '/api/User/Register', body,{headers : reqHeader});
  }

  userAuthentication(email, password) {
    var data = {'email':email,'password': password};
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/json','No-Auth':'True' });
    return this.http.post(this.rootUrl + '/auth', data, { headers: reqHeader });
  }

  getUsuariosClans(){
    const decodeToken = this.helper.decodeToken(localStorage.getItem('userToken'));
   return  this.http.get(this.rootUrl+'/UsuariosClans/'+decodeToken.email); 
  }

}
