import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Configuration } from './configuration';

import { AuthService } from './api/auth.service';
import { CategoriasService } from './api/categorias.service';
import { FornecedoresService } from './api/fornecedores.service';
import { LoginService } from './api/login.service';
import { PropostasService } from './api/propostas.service';
import { UsuariosService } from './api/usuarios.service';
import { UsuariosClansService } from './api/usuariosClans.service';

@NgModule({
  imports:      [ CommonModule, HttpClientModule ],
  declarations: [],
  exports:      [],
  providers: [
    AuthService,
    CategoriasService,
    FornecedoresService,
    LoginService,
    PropostasService,
    UsuariosService,
    UsuariosClansService ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        }
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import your base AppModule only.');
        }
    }
}
