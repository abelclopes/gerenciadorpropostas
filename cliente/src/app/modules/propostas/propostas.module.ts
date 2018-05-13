import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PropostasListComponent } from './propostas-list/propostas-list.component';
import { PropostasFormComponent } from './propostas-form/propostas-form.component';
import { PropostasEditComponent } from './propostas-edit/propostas-edit.component';
import { PropostaService } from './service/proposta.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AutocompleteModule } from 'ng2-input-autocomplete';
import { FornecedorService } from '../fornecedores/service/fornecedor.service';


const routes: Routes = [
  { path: '', component: PropostasListComponent },
  { path: 'editar/:id', component: PropostasEditComponent },
  { path: 'novo', component: PropostasFormComponent }
];
@NgModule({
  imports: [
    RouterModule.forChild(routes),
    NgxPaginationModule,
    CommonModule,
    FormsModule, 
    ReactiveFormsModule,
    NgbModule,
    AutocompleteModule.forRoot()
  ],
  declarations: [PropostasListComponent, PropostasFormComponent, PropostasEditComponent],
  providers:[
    PropostaService,FornecedorService
  ]
})
export class PropostasModule { }
