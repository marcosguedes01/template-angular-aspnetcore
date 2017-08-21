import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';

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
        this._http.get("/api/values").subscribe(values => {
            this.apiValues = values.json() as string[];
        });
    }
}
