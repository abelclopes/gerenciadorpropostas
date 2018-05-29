import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { UsuarioPagedListModel, UsuariosModel } from '../model';
import { UsuarioService } from '../service/usuario.service';
import { PerfilUsuarioEnum, PerfilUsuario, Perfil } from '../model';

@Component({
  selector: 'app-usuarios-list',
  templateUrl: './usuarios-list.component.html',
  styleUrls: ['./usuarios-list.component.css']
})
export class UsuariosListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  usuarios: UsuariosModel[]

  perfis:any[];
  paginaAtual: number;
  tamanhoPagina = 10
  totalItens: number;
  listaPagedListModel: UsuarioPagedListModel

  filter: UsuariosModel = new UsuariosModel();

  constructor(private catService: UsuarioService){
    this.paginaAtual = 1;
  }

  filtrar(termo: string): void {
    this.termoFiltro.next(termo);
  }

  paginar(pagina: number, termo: string): void {
    this.paginaAtual = pagina;
    this.termoFiltro.next(termo);
  }

  ngOnInit() {
      this.termoFiltro
      .debounceTime(200)
      .switchMap(termo => this.catService.getUsuarios(this.paginaAtual, this.tamanhoPagina,termo))
      .subscribe(x => {
        this.usuarios = x.resultado;
        this.totalItens = x.totalItens;
        console.log(this.usuarios[0]);
        
      })
      this.termoFiltro.next("");
      this.setPerfis();
  }
  delete(id: string){
    console.log(id);

    this.catService.delete(id)
    .subscribe(
      data => {
        if(data['ok'] == true)
          this.usuarios = this.usuarios.filter(x => x.id != id);
      }, err => {
        console.log(err);
      }
    );;
  }
  getUserPerfi(usuario: UsuariosModel){    
    let permissao = this.perfis.find(x=>x.id == usuario.perfilUsuario);
    return permissao.descricao;
  }

  setPerfis(){
    let  names: Perfil[] = [];
    let i:number =1;
    for(var n in PerfilUsuario) {
        if(typeof PerfilUsuario[n] === 'number') {          
          names.push({'id': i, 'descricao': n});
          i++;
        }
    }    
    localStorage.setItem('usuariosPerfis', JSON.stringify(names));
    this.perfis = names;
  }
}
