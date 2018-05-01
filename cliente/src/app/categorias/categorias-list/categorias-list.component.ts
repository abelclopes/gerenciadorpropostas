import { Component, OnInit } from '@angular/core';
import { Categoria,CategoriasService as CategoriasServiceApi } from '../../logica-api';
import { CategoriaService } from '../service/categoria.service'
import { PagedListModel } from '../../shared/paginacao/PagedListModel';
import { Subject } from 'rxjs';

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
  listaPaginadaCategoriasModel: Array<any>

  constructor(private catService: CategoriaService)
  {
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
    // this.catService.getCategorias()
    //   .subscribe(response => this.categorias = response);
    this.catService.getCategorias(this.paginaAtual, this.tamanhoPagina)
      this.termoFiltro
      .debounceTime(200)
      .switchMap(termo => this.catService.getCategorias(this.paginaAtual, this.tamanhoPagina))
      .subscribe(x => {
        this.categorias = x.resultado;
        this.totalItens = x.totalItems;
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
