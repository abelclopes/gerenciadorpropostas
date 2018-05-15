import { Component, OnInit } from '@angular/core';
import { CategoriaService } from '../service/categoria.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { CategoriaModel } from '../model/categoria.model';

@Component({
  selector: 'app-categorias-edit',
  templateUrl: './categorias-edit.component.html',
  styleUrls: ['./categorias-edit.component.css']
})
export class CategoriasEditComponent implements OnInit {
  categoria: CategoriaModel;
  categoriaForm: FormGroup
  nome: string;
  descricao: string;

  constructor(private catService: CategoriaService,private router : Router,
    private fb: FormBuilder, private route: ActivatedRoute) { }

  ngOnInit() {

    this.catService.getCategoriaById(this.route.snapshot.paramMap.get('id'))
    .subscribe(
      data => {
        this.categoria = data;
      }, err => {
        console.log(err);
      }
    );

    this.categoriaForm = this.fb.group({
      'nome': new FormControl(this.nome, [
        Validators.required,
        Validators.minLength(4)
      ]),
      'descricao': new FormControl(this.descricao, [
        Validators.required,
        Validators.minLength(4)
      ]),
    });

  }
  onSubmit(model){
    console.info('cadastrar nova categoria',model)
    this.categoria.nome = model.nome;
    this.categoria.descricao = model.descricao;
    this.catService.updateCategoria(this.categoria)
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
