﻿import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';

import { Globals } from '../globals';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'app';
    apiValues: string[] = [];

    constructor(private globals: Globals, private _http: Http) { }

    ngOnInit()
    {
        this._http.get("/api/values").subscribe(values => {
            this.apiValues = values.json() as string[];
        });
    }
}
