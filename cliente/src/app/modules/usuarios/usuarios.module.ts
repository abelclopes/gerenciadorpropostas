import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { UsuariosListComponent } from './usuarios-list/usuarios-list.component';
import { UsuariosEditComponent } from './usuarios-edit/usuarios-edit.component';
import { UsuariosFormComponent } from './usuarios-form/usuarios-form.component';
import { UsuarioService } from './service/usuario.service';
import { GeproMaskUtilService } from '../../shared/diretivas';
import { SharedModule } from '../../shared/shared.module';


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
    SharedModule,    
    FormsModule
  ],
  declarations: [UsuariosListComponent, UsuariosEditComponent, UsuariosFormComponent],
  providers:[
    UsuarioService
    ,GeproMaskUtilService
  ]
})
export class UsuariosModule { }
