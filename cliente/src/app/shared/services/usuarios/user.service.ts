import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Response } from "@angular/http";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { User } from './user.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../../environments/environment';
import { NovoUsuarioModel, UsuariosClansService } from '../../../logica-apis';
import { UsuarioAuthModel } from '../../../logica-apis/model/usuarioAuthModel';
@Injectable()
export class UserService {
  readonly rootUrl = environment.URL_API;
  jwtH = new JwtHelperService();

  constructor(private http: HttpClient,private usuariosClansService: UsuariosClansService) { }

  registrarUsuario(user: NovoUsuarioModel) {
    const body: NovoUsuarioModel = {
      nome: user.nome,
      senha: user.senha,
      email: user.email,
      perfilUsuario: user.perfilUsuario,
      dataNacimento: user.dataNacimento
    }
    var reqHeader = new HttpHeaders({'No-Auth':'True'});
    return this.http.post(this.rootUrl + '/api/User/Register', body,{headers : reqHeader});
  }

  userAuthentication(email, password) {
    var data = {'userID':email,'password': password};
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/json','No-Auth':'True' });
    return this.http.post(this.rootUrl + '/Login', data, { headers: reqHeader });
  }

  getUsuariosClans(){
    const decodeToken: any = this.jwtH.decodeToken(localStorage.getItem('accessToken'));
    console.log(decodeToken);
    var data = decodeToken.email;

    let reqHeader = new HttpHeaders({"Content-Type":"application/json","Accept":"application/json",'Authorization':`Bearer ${localStorage.getItem('accessToken')}`});
    return this.http.post<UsuarioAuthModel>(this.rootUrl + '/UsuariosClans/', data, {headers : reqHeader});
  }


  public isAuthenticaiton(){
    const user = localStorage.getItem('accessToken');
    if(user != undefined || user != null){
      return true
    }
    return false
  }


}
