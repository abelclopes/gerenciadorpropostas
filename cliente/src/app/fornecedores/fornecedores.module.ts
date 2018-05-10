import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'

import { FornecedoresListComponent } from './fornecedores-list/fornecedores-list.component';
import { FornecedoresEditarComponent } from './fornecedores-editar/fornecedores-editar.component';
import { FornecedoresFormComponent } from './fornecedores-form/fornecedores-form.component';

import { FornecedorService } from './service/fornecedor.service';


const routes: Routes = [
  { path: '', component: FornecedoresListComponent },
  { path: 'editar/:id', component: FornecedoresEditarComponent },
  { path: 'novo', component: FornecedoresFormComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    FornecedoresListComponent,
    FornecedoresEditarComponent,
    FornecedoresFormComponent
  ],
  providers:[
    FornecedorService
  ]
})
export class FornecedoresModule { }
