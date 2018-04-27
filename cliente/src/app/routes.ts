import { DashboardComponent } from './dashboard/dashboard.component';
import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component';
import { UsuarioCadastroComponent } from './usuarios/usuario-cadastro/usuario-cadastro.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { UsuariosComponent } from './usuarios/usuarios.component';

export const appRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent,canActivate:[AuthGuard] },
    { path: 'usuarios', component: UsuariosComponent,canActivate:[AuthGuard],  data: { expectedRole: 'admin'} },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'login', component: LoginComponent },
    { path : '', redirectTo:'/login', pathMatch : 'full'}
    
];
