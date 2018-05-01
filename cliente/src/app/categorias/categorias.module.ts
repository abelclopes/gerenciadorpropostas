import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { CategoriasListComponent } from './categorias-list/categorias-list.component';
import { CategoriasFormComponent } from './categorias-form/categorias-form.component';
import { CategoriasEditComponent } from './categorias-edit/categorias-edit.component';
import { CategoriaService } from './service/categoria.service';



const routes: Routes = [
  { path: '', component: CategoriasListComponent },
  { path: 'editar/:id', component: CategoriasEditComponent },
  { path: 'nova', component: CategoriasFormComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    CategoriasListComponent,
    CategoriasFormComponent,
    CategoriasEditComponent
  ],
  providers:[
    CategoriaService
  ]
})
export class CategoriasModule { }
