import { NgModule } from "@angular/core";
import { PropostasComponent } from "./component/propostas.component";
import { SharedModule } from "../shared/shared.module";
import { NgxPaginateModule } from 'ngx-paginate';
import { FormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { PropostaformComponent } from './component/propostaform/propostaform.component';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { NotFoundComponent } from "../not-found/not-found.component";


const ROUTES: Routes = [
  {path:'', component: PropostasComponent}
]

@NgModule({
  declarations:[
    PropostasComponent,
    PropostaformComponent
  ],
  imports: [
    SharedModule,
    NgxPaginateModule,
    FormsModule,
    RouterModule.forChild(ROUTES),
    BootstrapModalModule.forRoot({container:document.body})
  ]
})
export class PropostasModule {}
