import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthenticationService } from '../../services/authentication.service';

import 'rxjs/add/operator/finally';

import { Globals } from '../../globals';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [
        AuthenticationService
    ]
})
export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private globals: Globals) {

        this.globals.setLogged(false);
    }

    ngOnInit() {
        // reset login status
        this.authenticationService.logout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    login() {
        this.loading = true;

        this.authenticationService.login(this.model.username, this.model.password)
            .finally(() => this.loading = false)
            .subscribe(
            result => {
                if (result) {
                    this.globals.setLogged(true);
                    this.router.navigate([this.returnUrl]);
                }
            },
            error => {
                alert("Não foi possível efetuar o login.");
            });
    }
}
