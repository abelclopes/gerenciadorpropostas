
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/services/usuarios/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private router: Router, private userService: UserService) { }
  ngOnInit() {
  }

  title = 'app';
  isAuthenticaiton(){
    return this.userService.isAuthenticaiton()
  }
  
  Logout() {
    localStorage.removeItem('userToken');
    this.router.navigate(['/login']);
  }
}
