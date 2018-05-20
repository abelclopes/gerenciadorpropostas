import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Configuration } from './configuration';

import { AuthService } from './api/auth.service';
import { CategoriasService } from './api/categorias.service';
import { FornecedoresService } from './api/fornecedores.service';
import { PropostasService } from './api/propostas.service';
import { PropostasStatusService } from './api/propostasStatus.service';
import { UsuariosService } from './api/usuarios.service';
import { UsuariosClansService } from './api/usuariosClans.service';
import { UsuariosPermissoesService } from './api/usuariosPermissoes.service';

@NgModule({
  imports:      [ CommonModule, HttpClientModule ],
  declarations: [],
  exports:      [],
  providers: [
    AuthService,
    CategoriasService,
    FornecedoresService,
    PropostasService,
    PropostasStatusService,
    UsuariosService,
    UsuariosClansService,
    UsuariosPermissoesService ]
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
