
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../login/service/authentication.service';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { API_URL } from './../../app.api';
import { UsuarioBuilder } from '../../modules/usuarios/model/build';
import { UsuariosClans } from '../../modules/usuarios/model/usuario-clans.model';
import { tokenUser } from '../login/model/token-user';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  userClaims: UsuariosClans;
  email?:string;
  accessToken?: string;
  public defaultHeaders = new HttpHeaders();

  constructor(protected httpClient: HttpClient, private router: Router) { }  ngOnInit() {
      if(!this.isAuthenticaiton()){
        this.Logout();        
      }
  }
  isAuthenticaiton(){
    const authOn: tokenUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
    if(authOn != null && authOn != undefined){
      this.email = authOn['email'];
      this.accessToken = authOn['token'];
       return true
    }
    return false
  }
  Logout(){
    localStorage.clear();
    this.router.navigate(['/logout']);
  }
}
