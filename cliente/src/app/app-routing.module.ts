import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HttpModule } from '@angular/http';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path : '', redirectTo:'/login', pathMatch : 'full'},
  { path: 'login', loadChildren: './components/login/login.module#LoginModule' },
  { path: 'fornecedores', loadChildren: './modules/fornecedores/fornecedores.module#FornecedoresModule', canActivate: [AuthGuard]},
  { path: 'categorias', loadChildren: './modules/categorias/categorias.module#CategoriasModule', canActivate: [AuthGuard]},
  { path: 'propostas', loadChildren: './modules/propostas/propostas.module#PropostasModule', canActivate: [AuthGuard]},
  { path: 'usuarios', loadChildren: './modules/usuarios/usuarios.module#UsuariosModule', canActivate: [AuthGuard]},
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules}),
    CommonModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
