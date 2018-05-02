export * from './auth.service';
import { AuthService } from './auth.service';
export * from './categorias.service';
import { CategoriasService } from './categorias.service';
export * from './fornecedores.service';
import { FornecedoresService } from './fornecedores.service';
export * from './propostas.service';
import { PropostasService } from './propostas.service';
export * from './usuarios.service';
import { UsuariosService } from './usuarios.service';
export * from './usuariosClans.service';
import { UsuariosClansService } from './usuariosClans.service';
export const APIS = [AuthService, CategoriasService, FornecedoresService, PropostasService, UsuariosService, UsuariosClansService];
