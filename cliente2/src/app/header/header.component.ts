
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../login/service/authentication.service';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import { CustomHttpUrlEncodingCodec } from '../logica-api/encoder';
import { Observable } from 'rxjs/Observable';
import { API_URL } from '../app.api';
import { HeaaderService } from './header.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  userClaims: any;
  email?:string;
  accessToken?: string;
  public defaultHeaders = new HttpHeaders();

  constructor(protected httpClient: HttpClient, private router: Router, private headerService: HeaaderService) { }
  ngOnInit() {
    this.isAuthenticaiton();
    console.log('chama teste')
    this.headerService.UsuariosClans()
    .subscribe(
      data =>{
        this.userClaims = data
        localStorage.setItem('userDetails', JSON.stringify(data))
        console.log(this.userClaims)
    });
    this.userClaims = this.headerService.response;
  }
  isAuthenticaiton(){
    const authOn = JSON.parse(localStorage.getItem('usuarioCorrente'));
    if(authOn != null){
      this.email = authOn['email'];
      this.accessToken = authOn['token'];
       return true
    }
    return false
  }
  Logout(){
    this.router.navigate(['/logout']);
  }
}
