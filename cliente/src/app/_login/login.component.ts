import { Component, OnInit } from '@angular/core';
import { UserService } from './../shared/services/usuarios/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { DialogService } from 'ng2-bootstrap-modal';
import { DialogModalComponent } from '../shared/dialog-modal/dialog-modal.component';
import { NotificationService } from '../shared/messages/notification.service';
import { Observable } from 'rxjs/Observable';
import { AuthService, LoginModel } from '../logica-apis';
import { API } from '../app.api';
import { tokenModel } from '../shared/services/token.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  isLoginError : boolean = false;
  ImageUrl: string = API + '/assets/faviicon.png';
  email;
  password;
  response: tokenModel;
  constructor(private userService : UserService,private authService: AuthService , private router : Router,  private notificationService: NotificationService) { }

  ngOnInit() {
    if(this.userService.isAuthenticaiton()) this.router.navigate(['/dashboard']);
  }

  OnSubmit(email,password){

     this.userService.userAuthentication(email, password)
     .subscribe(
        data => {
        const response  = JSON.stringify(data['accessToken']);

        localStorage.setItem('accessToken',response);
        if(response['status'] != 401){
          this.router.navigate(['/dashboard']);
        }else{
          this.notificationService.notify('Usuario ou senha são invalidos')
        }
    },
    err =>{
      const response = JSON.stringify(err);

      if(response['status'] == 401){
        this.notificationService.notify(response)
      }
    });
  }

}
