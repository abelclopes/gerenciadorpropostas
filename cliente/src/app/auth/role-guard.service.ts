// src/app/auth/role-guard.service.ts
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { JwtHelperService} from '@auth0/angular-jwt';
@Injectable()
export class RoleGuardService implements CanActivate {

    constructor(public auth: AuthService, public router: Router,public jwtHelper: JwtHelperService) {}
    
    canActivate(route: ActivatedRouteSnapshot): boolean {
        
        const expectedRole = route.data.expectedRole;
        const token = localStorage.getItem('userToken');
        
        const tokenPayload = this.jwtHelper.decodeToken(token);
        if (  !this.auth.isAuthenticated() ||  tokenPayload.GivenName !== expectedRole ) {
            alert('a sessão expirou!');
            this.router.navigate(['login']);
            return false;
        }
        return true;
    }
}