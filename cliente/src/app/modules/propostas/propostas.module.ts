import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule, Routes } from '@angular/router';
import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PropostasListComponent } from './propostas-list/propostas-list.component';
import { PropostasFormComponent } from './propostas-form/propostas-form.component';
import { PropostasEditComponent } from './propostas-edit/propostas-edit.component';
import { PropostaService } from './service/proposta.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FornecedorService } from '../fornecedores/service/fornecedor.service';
import { PropostaDetalhesComponent } from './proposta-detalhes/proposta-detalhes.component';
import { CurrencyPipe } from '@angular/common';
import { NgxCurrencyModule } from "ngx-currency";

import localePtBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
registerLocaleData(localePtBr);


import { PdfViewerModule } from 'ng2-pdf-viewer';


const routes: Routes = [
  { path: '', component: PropostasListComponent },
  { path: 'editar/:id', component: PropostasEditComponent },
  { path: 'detalhes/:id', component: PropostaDetalhesComponent },
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
    NgxCurrencyModule,
    PdfViewerModule
  ],
  declarations: [PropostasListComponent, PropostasFormComponent, PropostasEditComponent, PropostaDetalhesComponent],
  providers:[
    PropostaService,FornecedorService,
    {provide: LOCALE_ID, useValue: 'pt'},
  ]
})
export class PropostasModule { }
