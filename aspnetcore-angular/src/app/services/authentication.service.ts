import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class AuthenticationService {
    constructor(private http: Http) { }

    login(userName, password) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this.http
            .post('/api/auth/login', JSON.stringify({ userName, password }), { headers })
            .map(res => res.json())
            .map(res => {
                if (res.auth_token) {
                    localStorage.setItem('auth_token', res.auth_token);
                    localStorage.setItem('currentUser', JSON.stringify(userName));
                }
                return true;
            })
            .catch(this.handleError);
    }

    logout() {
        localStorage.clear();
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}