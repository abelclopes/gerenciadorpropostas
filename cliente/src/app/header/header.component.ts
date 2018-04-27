
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
    this.userService.getUsuariosClans().subscribe((data: any) => {
      this.userClaims = data;
    });
  }

  title = 'app';
  isAuthenticaiton(){
    return this.userService.isAuthenticaiton()
  }
  shoeMenuRole(){
    this.userClaims.police
  }
  Logout() {
    localStorage.removeItem('userToken');
    this.router.navigate(['/login']);
  }
}
