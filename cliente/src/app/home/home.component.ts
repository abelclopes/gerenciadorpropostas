import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/services/usuarios/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userClaims: any;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.userService.getUsuariosClans().subscribe((data: any) => {
      this.userClaims = data;
    });
  }


}
