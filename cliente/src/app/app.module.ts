import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, PreloadAllModules } from '@angular/router'

import { AppComponent } from './app.component';
import { UserService } from './shared/services/usuarios/user.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr'
import { UsuarioCadastroComponent } from './usuarios/usuario-cadastro/usuario-cadastro.component';
import { appRoutes } from './routes';
import { AuthGuard } from './auth/auth.guard';
import { AuthInterceptor } from './auth/auth.interceptor';
import { UsuariosComponent } from './usuarios/usuarios.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HeaderComponent } from './header/header.component';
import { FornecedoresComponent } from './fornecedores/fornecedores.component';
import { CategoriasComponent } from './categorias/categorias.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { PropostasApi } from './logica-apis';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    UsuarioCadastroComponent,
    UsuariosComponent,
    LoginComponent,
    DashboardComponent,
    HeaderComponent,
    FornecedoresComponent,
    CategoriasComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    SharedModule.forRoot(),
    RouterModule.forRoot(appRoutes, {preloadingStrategy: PreloadAllModules})
  ],
  providers: [
    UserService,
    AuthGuard,
    PropostasApi,
    {
      provide : HTTP_INTERCEPTORS,
      useClass : AuthInterceptor,
      multi : true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
