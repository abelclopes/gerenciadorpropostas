import { NgModule } from "@angular/core";
import { PropostasComponent } from "./component/propostas.component";
import { SharedModule } from "../shared/shared.module";
import { NgxPaginateModule } from 'ngx-paginate';
import { FormsModule } from "@angular/forms";
import { ModalModule } from 'ng2-modal-module';
import { RouterModule, Routes } from "@angular/router";
import { PropostaformComponent } from './component/propostaform/propostaform.component';


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
    ModalModule,
    RouterModule.forChild(ROUTES)]
})
export class CompromissosModule {}
