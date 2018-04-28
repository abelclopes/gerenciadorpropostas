import { FornecedoresComponent } from './fornecedores/fornecedores.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { Routes } from '@angular/router'
import { UsuarioCadastroComponent } from './usuarios/usuario-cadastro/usuario-cadastro.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { UsuariosComponent } from './usuarios/usuarios.component';
import { CategoriasComponent } from './categorias/categorias.component';
import { NotFoundComponent } from './not-found/not-found.component';

export const appRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent,canActivate:[AuthGuard] },
    { path: 'usuarios', component: UsuariosComponent,canActivate:[AuthGuard],  data: { expectedRole: ['Administrador']} },
    { path: 'fornecedores', component: FornecedoresComponent,canActivate:[AuthGuard],  data: { expectedRole: ['Administrador']} },
    { path: 'categorias', component: CategoriasComponent,canActivate:[AuthGuard],  data: { expectedRole: ['Administrador']} },
    { path: 'propostas', component: 'propostas'},
    { path: 'login', component: LoginComponent },
    { path : '', redirectTo:'/login', pathMatch : 'full'}

];
