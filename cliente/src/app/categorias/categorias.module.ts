import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { CategoriasListComponent } from './categorias-list/categorias-list.component';
import { CategoriasFormComponent } from './categorias-form/categorias-form.component';
import { CategoriasDetalhesComponent } from './categorias-detalhes/categorias-detalhes.component';
import { CategoriasEditComponent } from './categorias-edit/categorias-edit.component';



const routes: Routes = [
  //{ path : '', redirectTo:'/login', pathMatch : 'full'},
  { path: '', component: CategoriasListComponent },
  { path: 'categorias', component: CategoriasListComponent}
  { path: 'categorias/nova', component: CategoriasFormComponent},
  { path: 'categorias/edit/:id', component: CategoriasFormComponent},
  { path: 'categorias/datalhes/:id', component: CategoriasDetalhesComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule
  ],
  declarations: [CategoriasListComponent, CategoriasFormComponent, CategoriasDetalhesComponent, CategoriasEditComponent]
})
export class CategoriasModule { }
