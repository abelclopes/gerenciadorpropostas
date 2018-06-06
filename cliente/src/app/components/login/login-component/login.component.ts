import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from '../service/authentication.service';
import { LoginModel } from '../../../logica-api';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string;
  password: string;
  loading = false;
  error =false;
  constructor(private authService: AuthenticationService, private router : Router) { }

  ngOnInit() {
  }
  onSubmit(email, password)
  {
    this.authService.login(email, password)
    .subscribe(data => {
      if(data == true)
      this.authService.UsuariosClans()
        .subscribe(
          data =>{            
            localStorage.setItem('usuarioClans', JSON.stringify(data))
            this.router.navigate(['/dashboard']);
        });
    });
  }
}
