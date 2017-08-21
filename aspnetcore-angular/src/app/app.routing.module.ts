import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Fonte: https://github.com/cornflourblue/angular2-registration-login-example
//        http://jasonwatmore.com/post/2016/09/29/angular-2-user-registration-and-login-example-tutorial
import { AuthGuard } from './security/auth.guard';

import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

const routes: Routes = [
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }

// Exemplo de breadcrumb com rotas
// http://brianflove.com/2016/10/23/angular2-breadcrumb-using-router/