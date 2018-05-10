import { Component, OnInit } from '@angular/core';
import { FornecedorModel } from '../model/fornecedor.model';
import { Subject } from 'rxjs';
import { PagedListModel } from '../../shared/paginacao/PagedListModel';
import { FornecedoresModule } from '../fornecedores.module';
import { FornecedorService } from '../service/fornecedor.service';

@Component({
  selector: 'app-fornecedores-list',
  templateUrl: './fornecedores-list.component.html',
  styleUrls: ['./fornecedores-list.component.css']
})
export class FornecedoresListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  fornecedores: FornecedorModel[]


  paginaAtual: number;
  tamanhoPagina = 10
  totalItens: number;
  listaPagedListModel: PagedListModel

  filter: FornecedoresModule = new FornecedorModel();

  constructor(private catService: FornecedorService){
    this.paginaAtual = 1;
  }

  filtrar(termo: string): void {
    this.termoFiltro.next(termo['path'][0].value);
  }

  paginar(pagina: number, termo: string): void {
    this.paginaAtual = pagina;
    this.termoFiltro.next(termo);
  }

  ngOnInit() {
      this.termoFiltro
      .debounceTime(200)
      .switchMap(termo => this.catService.getFornecedors(this.paginaAtual, this.tamanhoPagina,termo))
      .subscribe(x => {
        this.fornecedores = x.resultado;
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
          this.fornecedores = this.fornecedores.filter(x => x.id != id);
      }, err => {
        console.log(err);
      }
    );;
  }
}
