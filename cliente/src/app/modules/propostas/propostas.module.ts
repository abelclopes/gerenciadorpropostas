import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PropostasListComponent } from './propostas-list/propostas-list.component';
import { PropostasFormComponent } from './propostas-form/propostas-form.component';
import { PropostasEditComponent } from './propostas-edit/propostas-edit.component';
import { PropostaService } from './service/proposta.service';


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
  ],
  declarations: [PropostasListComponent, PropostasFormComponent, PropostasEditComponent],
  providers:[
    PropostaService
  ]
})
export class PropostasModule { }
