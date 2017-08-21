import { Component, OnInit } from '@angular/core';
import { Headers, Http } from "@angular/http";

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    title = 'app';
    apiValues: string[] = [];
    
    constructor(private _http: Http) { }
    
    ngOnInit() {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = localStorage.getItem('auth_token');
        headers.append('Authorization', `Bearer ${authToken}`);

        this._http.get("/api/values", { headers }).subscribe(values => {
            this.apiValues = values.json() as string[];
        });
    }
}
