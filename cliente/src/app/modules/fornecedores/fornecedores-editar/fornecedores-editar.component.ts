import { Component, OnInit } from '@angular/core';
import { FornecedorModel } from '../model';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { FornecedorService } from '../service/fornecedor.service';
import { Router, ActivatedRoute } from '@angular/router';
import { GeproMaskUtilService } from '../../../shared/diretivas';

@Component({
  selector: 'app-fornecedores-editar',
  templateUrl: './fornecedores-editar.component.html',
  styleUrls: ['./fornecedores-editar.component.css'],
  moduleId: module.id.toString()
})
export class FornecedoresEditarComponent implements OnInit {
  public maskTelefone = GeproMaskUtilService.DYNAMIC_PHONE_MASK_GENERATOR;
  public maskCnpj = GeproMaskUtilService.DYNAMIC_PERSON_MASK_GENERATOR;
  
  fornecedor: FornecedorModel;
  fornecedorForm: FormGroup

  constructor(private catService: FornecedorService,
    private fb: FormBuilder, private router : Router, private route: ActivatedRoute) { }
    
  ngOnInit() {
    this.catService.getFornecedorById(this.route.snapshot.paramMap.get('id'))
    .subscribe(
      data => {
        this.fornecedor = data;
      }, err => {
        console.log(err);
      }
    );

    this.fornecedorForm = this.fb.group({
      'nome': new FormControl('', [Validators.required, Validators.minLength(4)]),
      'cnpjCpf': new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(19) ]),
      'telefone': new FormControl('', [ Validators.required, Validators.minLength(4) ]),
      'email': new FormControl('', [
        Validators.required,
        Validators.minLength(4)
      ])
    });

  }
  onSubmit(model){
    console.info('cadastrar nova fornecedor',model)
    this.fornecedor.nome = model.nome;
    this.fornecedor.cnpjCpf = model.cnpjCpf.replace(/[^\d]+/g,'');
    this.fornecedor.email = model.email;
    this.fornecedor.telefone = model.telefone;
    this.catService.updateFornecedor(this.fornecedor)
      .subscribe(
        data => {
          if(data['ok'] == true)
            this.router.navigate(['/fornecedores']);
        }, err => {
          console.log(err);
        }
      );
  }
  error: boolean = false; 
  errors(){
    return this.error;
  }
  resetForm(){}
}
