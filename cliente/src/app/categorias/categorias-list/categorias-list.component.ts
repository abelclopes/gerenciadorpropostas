import { Component, OnInit, ViewChild } from '@angular/core';
import { Categoria,CategoriasService as CategoriasServiceApi } from '../../logica-api';
import { CategoriaService } from '../service/categoria.service'
import { PagedListModel } from '../../shared/paginacao/PagedListModel';
import { Subject } from 'rxjs';
import { CategoriaModel } from '../model/categoria.model';
import { DatatableComponent } from '@swimlane/ngx-datatable';

@Component({
  selector: 'app-categorias-list',
  templateUrl: './categorias-list.component.html',
  styleUrls: ['./categorias-list.component.css']
})
export class CategoriasListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  categorias: Categoria[]


  paginaAtual: number;
  tamanhoPagina = 10
  totalItens: number;
  listaPagedListModel: PagedListModel

  filter: Categoria = new CategoriaModel();

  constructor(private catService: CategoriaService){
    this.paginaAtual = 1;
  }
  @ViewChild(DatatableComponent) table: DatatableComponent;

  filtrar(termo: string): void {
    this.termoFiltro.next(termo['path'][0].value);
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
