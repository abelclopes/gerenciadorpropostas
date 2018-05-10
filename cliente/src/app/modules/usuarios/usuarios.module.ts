import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsuariosListComponent } from './usuarios-list/usuarios-list.component';
import { UsuariosEditComponent } from './usuarios-edit/usuarios-edit.component';
import { UsuariosFormComponent } from './usuarios-form/usuarios-form.component';
import { UsuarioService } from './service/usuario.service';

import { 
  FormsModule,
  ReactiveFormsModule }   from '@angular/forms';
const routes: Routes = [
  { path: '', component: UsuariosListComponent },
  { path: 'editar/:id', component: UsuariosEditComponent },
  { path: 'novo', component: UsuariosFormComponent }
];
@NgModule({
  imports: [
    RouterModule.forChild(routes),
    NgxPaginationModule,
    CommonModule,
    FormsModule, 
    ReactiveFormsModule,   
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [UsuariosListComponent, UsuariosEditComponent, UsuariosFormComponent],
  providers:[
    UsuarioService
  ]
})
export class UsuariosModule { }
