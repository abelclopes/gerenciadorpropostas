import { Component, Input, OnChanges, OnInit, EventEmitter, Output, ViewChild, ViewEncapsulation  } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { PropostaModel } from '../model';
import { PropostaService } from '../service/proposta.service';
@Component({
  selector: 'app-propostas-form',
  templateUrl: './propostas-form.component.html',
  styleUrls: ['./propostas-form.component.css']
})
export class PropostasFormComponent implements OnInit {
  @Input() proposta: PropostaModel;
  @Output() created: EventEmitter<PropostaModel> = new EventEmitter<PropostaModel>();
  @Output() updated: EventEmitter<PropostaModel> = new EventEmitter<PropostaModel>();
  @ViewChild("propostas") propostas: any;

  CategoriaOptions: {id:number, nome:string}[];

  propostaForm: FormGroup;

  get nomeProposta() { return this.propostaForm.get('nomeProposta'); }
  get descricao() { return this.propostaForm.get('descricao'); }
  get fornecedor() { return this.propostaForm.get('fornecedor'); }
  get categoria() { return this.propostaForm.get('categoria'); }
  get anexo() { return this.propostaForm.get('anexo'); }
  get valor() { return this.propostaForm.get('valor'); }


  constructor(private propostaService: PropostaService, private router : Router, private fb: FormBuilder) { }

  ngOnInit() {
    this.propostaForm =  this.fb.group({
        nomeProposta: ['', Validators.required ],
        descricao: ['', Validators.required ],
        fornecedor: ['', Validators.required ],
        categoria: ['', Validators.required ],
        anexo: new FormControl(null),
        valor: ['', Validators.required ]
    }, { updateOn: 'submit' });
  }
  onSubmit() {
    if (this.propostaForm.valid) {
        this.propostaService.createUpload(this.prepareSaveUpload()).subscribe((proposta) => this.created.emit(proposta));
    }
  }
  prepareSaveUpload(): FormData {
    const formModel = this.propostaForm.value;

      let formData = new FormData();
      formData.append("propostaForm", formModel.propostaForm);
      formData.append("descricao", formModel.descricao);
      formData.append("fornecedor", formModel.fornecedor);
      formData.append("categoria", formModel.categoria);
      formData.append("anexo", formModel.anexo);
      formData.append("valor", formModel.valor);
      
      return formData;
  }
  fileChange(files: FileList) {
      if (files && files[0].size > 0) {
          this.propostaForm.patchValue({
              pdf: files[0]
          });
      }
  }
}
