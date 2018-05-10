import { Component, OnInit } from '@angular/core';
import { CategoriaService } from '../service/categoria.service'
import { Subject } from 'rxjs';
import { CategoriaPagedListModel, CategoriaModel } from '../model';

@Component({
  selector: 'app-categorias-list',
  templateUrl: './categorias-list.component.html',
  styleUrls: ['./categorias-list.component.css']
})
export class CategoriasListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  categorias: CategoriaModel[]


  paginaAtual: number;
  tamanhoPagina = 10
  totalItens: number;
  listaPagedListModel: CategoriaPagedListModel

  filter: CategoriaModel = new CategoriaModel();

  constructor(private catService: CategoriaService){
    this.paginaAtual = 1;
  }

  filtrar(termo: string): void {
    this.termoFiltro.next(termo);
  }

  paginar(pagina: number, termo: string): void {
    console.log(termo)
    this.paginaAtual = pagina;
    this.termoFiltro.next(termo);
    console.log(pagina)
  }

  ngOnInit() {
    this.termoFiltro
    .debounceTime(200)
    .switchMap(termo => this.catService.getCategorias(this.paginaAtual, this.tamanhoPagina,termo))
    .subscribe(x => {
      this.categorias = x.resultado;
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
          this.categorias = this.categorias.filter(x => x.id != id);
      }, err => {
        console.log(err);
      }
    );;
  }
}
