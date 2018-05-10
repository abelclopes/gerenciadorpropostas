import { Component, OnInit } from '@angular/core';
import { PropostaModel } from '../model/proposta.model';
import { Subject } from 'rxjs';
import { PagedListModel } from '../../shared/paginacao/PagedListModel';
import { PropostasModule } from '../propostas.module';
import { PropostaService } from '../service/proposta.service';

@Component({
  selector: 'app-propostas-list',
  templateUrl: './propostas-list.component.html',
  styleUrls: ['./propostas-list.component.css']
})
export class PropostasListComponent implements OnInit {

  private termoFiltro = new Subject<string>();
  propostas: PropostaModel[]


  paginaAtual: number;
  tamanhoPagina = 10
  totalItens: number;
  listaPagedListModel: PagedListModel

  filter: PropostasModule = new PropostaModel();

  constructor(private catService: PropostaService){
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
      .switchMap(termo => this.catService.getPropostas(this.paginaAtual, this.tamanhoPagina,termo))
      .subscribe(x => {
        this.propostas = x.resultado;
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
          this.propostas = this.propostas.filter(x => x.id != id);
      }, err => {
        console.log(err);
      }
    );;
  }
}
