import { Component, OnInit } from '@angular/core';
import { UserService } from './../shared/services/usuarios/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  isLoginError : boolean = false;
  ImageUrl: string = 'http://localhost:4200/assets/faviicon.png';
  email;
  password;
  constructor(private userService : UserService,private router : Router) { }

  ngOnInit() {
    if(this.userService.isAuthenticaiton()) this.router.navigate(['/dashboard']);
  }

  OnSubmit(Email,password){
     this.userService.userAuthentication(Email,password).subscribe((data : any)=>{
      localStorage.setItem('userToken',data.token);
      this.router.navigate(['/dashboard']);
    },
    (err : HttpErrorResponse)=>{
      this.isLoginError = true;
    });
  }

}
