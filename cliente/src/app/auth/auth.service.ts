// src/app/auth/auth.service.ts
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable()
export class AuthService {
  static jwtHelpers:  JwtHelperService;
  static isAuthenticated(): any {
    const token = localStorage.getItem('userToken');
    return !this.jwtHelpers.isTokenExpired(token);
  }
  constructor(public jwtHelper: JwtHelperService) {}

  public isAuthenticated(): boolean {
    const token = localStorage.getItem('userToken');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
