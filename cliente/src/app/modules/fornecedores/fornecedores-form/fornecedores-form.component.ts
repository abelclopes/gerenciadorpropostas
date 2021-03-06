import { Component, OnInit } from '@angular/core';

import { FornecedorService } from '../service/fornecedor.service'
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FornecedorModel } from '../model/fornecedor.model';

import { GeproMaskUtilService } from '../../../shared/diretivas';

@Component({
  selector: 'app-fornecedores-form',
  templateUrl: './fornecedores-form.component.html',
  styleUrls: ['./fornecedores-form.component.css']
})
export class FornecedoresFormComponent implements OnInit {
  public maskTelefone = GeproMaskUtilService.DYNAMIC_PHONE_MASK_GENERATOR;
  public maskCpfCnpfj = GeproMaskUtilService.DYNAMIC_PERSON_MASK_GENERATOR;
  

  nome: string;
  descricao: string;
  fornecedorForm: FormGroup;
  model: FornecedorModel;
  constructor(private catService: FornecedorService,private router : Router) { }

  ngOnInit() {

    this.fornecedorForm = new FormGroup({
      'nome': new FormControl(this.nome, [
        Validators.required,
        Validators.minLength(4)
      ]),
      'email': new FormControl(this.descricao, [
        Validators.required,
        Validators.minLength(4),
        Validators.email
      ]),
      'cnpjCpf': new FormControl(this.descricao, [
        Validators.required,
        Validators.minLength(11)
      ]),
      'telefone': new FormControl(this.descricao, [
        Validators.required,
        Validators.minLength(9)
      ])
    });
  }
  onSubmit(model){
    console.info('cadastrar nova fornecedor', model)
    model = new FornecedorModel();
    let novaFornecedor: FornecedorModel;
    model.nome = this.fornecedorForm.controls['nome'].value;
    model.email = this.fornecedorForm.controls['email'].value;
    model.cnpjCpf = this.fornecedorForm.controls['cnpjCpf'].value.replace(/[^0-9]+/g,'');;
    model.telefone = this.fornecedorForm.controls['telefone'].value.replace(/[^0-9]+/g,'');;
    this.catService.novaFornecedor(model)
      .subscribe(
        data => {
          if(data['ok'] == true)
            this.router.navigate(['/fornecedores']);
        }, err => {
          console.log(err);
        }
      );
  }
  errors(){}
  resetForm(){}
}
