
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/services/usuarios/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  userClaims: any;

  constructor(private router: Router, private userService: UserService) { }
  ngOnInit() {
    const token = localStorage.getItem('accessToken');
    console.log(token)
    if(token == null || token == undefined || token == 'undefined')
    {
      this.Logout();
    }
    else
    {
      this.userService.getUsuariosClans()
      .subscribe(
        data => this.userClaims = data);
    }
  }

  title = 'app';
  isAuthenticaiton(){
    return this.userService.isAuthenticaiton()
  }
  shoeMenuRole(){
    this.userClaims.police
  }
  Logout() {
    localStorage.removeItem('accessToken');
    this.router.navigate(['/login']);
  }
}
