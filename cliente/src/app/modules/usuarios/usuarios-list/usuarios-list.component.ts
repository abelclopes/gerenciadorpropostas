import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { UsuarioPagedListModel, UsuariosModel } from '../model';
import { UsuarioService } from '../service/usuario.service';

@Component({
  selector: 'app-usuarios-list',
  templateUrl: './usuarios-list.component.html',
  styleUrls: ['./usuarios-list.component.css']
})
export class UsuariosListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  usuarios: UsuariosModel[]


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
      })
      this.termoFiltro.next("");
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
}
