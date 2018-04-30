import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/services/usuarios/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    const token = localStorage.getItem('accessToken');
    if(token == null || token == undefined || token == 'undefined')
    {
      this.Logout();
    }
  }


  Logout() {
    localStorage.removeItem('accessToken');
    this.router.navigate(['/login']);
  }
}
