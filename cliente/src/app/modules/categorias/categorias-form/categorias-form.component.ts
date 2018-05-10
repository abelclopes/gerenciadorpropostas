import { Component, OnInit } from '@angular/core';

import { CategoriaService } from '../service/categoria.service'
import { Router } from '@angular/router';
import { CategoriaModel } from '../model/categoria.model';
@Component({
  selector: 'app-categorias-form',
  templateUrl: './categorias-form.component.html',
  styleUrls: ['./categorias-form.component.css']
})
export class CategoriasFormComponent implements OnInit {
  nome: string;
  descricao: string;
  constructor(private catService: CategoriaService,private router : Router) { }

  ngOnInit() {
  }
  onSubmit(nome: string, descricao: string){
    console.info('cadastrar nova categoria', nome + ' - ' + descricao)
    let novaCategoria: CategoriaModel;
    this.catService.novaCategoria(nome, descricao)
      .subscribe(
        data => {
          if(data['ok'] == true)
            this.router.navigate(['/categorias']);
        }, err => {
          console.log(err);
        }
      );
  }

}
