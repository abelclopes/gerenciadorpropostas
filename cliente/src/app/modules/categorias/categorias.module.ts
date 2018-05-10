import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { CategoriasListComponent } from './categorias-list/categorias-list.component';
import { CategoriasFormComponent } from './categorias-form/categorias-form.component';
import { CategoriasEditComponent } from './categorias-edit/categorias-edit.component';

import { CategoriaService } from './service/categoria.service';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';


import { NgxPaginationModule } from 'ngx-pagination';



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
    NgxDatatableModule,
    NgxPaginationModule,
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
