import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FornecedorService } from '../service/fornecedor.service';
import { FornecedorPagedListModel,FornecedorModel } from '../model';
import { FornecedoresModule } from '../fornecedores.module';

@Component({
  selector: 'app-fornecedores-list',
  templateUrl: './fornecedores-list.component.html',
  styleUrls: ['./fornecedores-list.component.css']
})
export class FornecedoresListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  fornecedores: FornecedorModel[]


  paginaAtual: number;
  tamanhoPagina = 10;
  totalItens: number;
  listaPagedListModel: FornecedorPagedListModel;

  filter: FornecedoresModule = new FornecedorModel();

  constructor(private catService: FornecedorService) {
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
      .switchMap(termo => this.catService.getFornecedors(this.paginaAtual, this.tamanhoPagina,termo))
      .subscribe(x => {
        this.fornecedores = x.resultado;
        this.totalItens = x.totalItens;
      })
      this.termoFiltro.next('');
  }
  delete(id: string) {
    console.log(id);

    this.catService.delete(id)
    .subscribe(data => {
        if (data['ok'] == true) {
          this.fornecedores = this.fornecedores.filter(x => x.id != id);
        }
      },err => { console.log(err); });
  }
}
