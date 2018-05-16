import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

import { PropostasListComponent } from './propostas-list/propostas-list.component';
import { PropostasFormComponent } from './propostas-form/propostas-form.component';
import { PropostasEditComponent } from './propostas-edit/propostas-edit.component';
import { PropostaService } from './service/proposta.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FornecedorService } from '../fornecedores/service/fornecedor.service';

import { CurrencyMaskModule } from "ng2-currency-mask";
import { CurrencyMaskConfig, CURRENCY_MASK_CONFIG } from "ng2-currency-mask/src/currency-mask.config";


export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  decimal: ",",
  precision: 2,
  prefix: "",
  suffix: "",
  thousands: "."
};


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
    SharedModule,
    CurrencyMaskModule
  ],
  declarations: [PropostasListComponent, PropostasFormComponent, PropostasEditComponent],
  providers:[
    PropostaService,FornecedorService,
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig }
  ]
})
export class PropostasModule { }
