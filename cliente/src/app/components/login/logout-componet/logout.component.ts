import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from '../service/authentication.service';
import { LoginModel } from '../../../logica-api';

@Component({
  selector: 'app-logout',
  template: '<div></div>',
})
export class LogoutComponent implements OnInit {
  constructor(private authService: AuthenticationService, private router : Router) { }

  ngOnInit() {
    localStorage.removeItem('usuarioCorrente');
    this.router.navigate(['/login']);

  }
  Logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
