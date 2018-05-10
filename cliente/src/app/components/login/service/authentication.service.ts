import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from './../../../app.api'

@Injectable()
export class AuthenticationService {
    public token: string;

    constructor(private http: Http) {
        // set token if saved in local storage
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
    }

    login(email: string, password: string): Observable<boolean> {
        return this.http.post(API_URL +'/api/auth', { 'email': email, 'password': password })
            .map((response: Response) => {
                  let token = response.json() && response.json().token;
                if (token) {
                    this.token = token;
                    localStorage.setItem('usuarioCorrente', JSON.stringify({ email: email, token: token }));
                    return true;
                } else {
                    return false;
                }
            });
    }
    logout(): void {
        this.token = null;
        localStorage.removeItem('usuarioCorrente');
    }
}
