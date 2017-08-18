import { Injectable } from '@angular/core';

@Injectable()
export class Globals {
    logged: boolean = false;
    
    constructor() {
        var _globals = this.getGlobalsStorage();
        this.logged = _globals.logged;
    }

    setLogged(logged: boolean): void {
        var _globals = this.getGlobalsStorage();

        _globals.logged = logged;
        this.logged = logged;

        localStorage.setItem('globals', JSON.stringify(_globals));
    }

    private getGlobalsStorage(): Globals {
        var _globals: Globals = this;

        if (localStorage.getItem('globals')) {
            _globals = JSON.parse(localStorage.getItem('globals')) as Globals;
        }

        return _globals;
    }
}