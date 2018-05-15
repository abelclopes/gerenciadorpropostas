import { Component, Input, OnChanges, OnInit, EventEmitter, Output, ViewChild, ViewEncapsulation  } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { PropostaModel } from '../model';
import { PropostaService } from '../service/proposta.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { LoadingService } from '../../../LoadingService';
import { FornecedorModel } from '../../fornecedores/model';
import { FornecedorService } from '../../fornecedores/service/fornecedor.service';
import { Observable, Subject } from 'rxjs';  

@Component({
  moduleId: module.id.toString(),
  selector: 'app-propostas-form',
  templateUrl: './propostas-form.component.html',
  styleUrls: ['./propostas-form.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class PropostasFormComponent implements OnInit {
  inputChanged: string;
  selectedItem: any;
  
  @Input() proposta: PropostaModel;
  @Output() created: EventEmitter<PropostaModel> = new EventEmitter<PropostaModel>();
  @Output() updated: EventEmitter<PropostaModel> = new EventEmitter<PropostaModel>();
  @ViewChild("uploader") uploader: any;

  CategoriaOptions: {id:number, nome:string}[];
   
  public fornecedores: any[] = [];
  private termoFiltro = new Subject<string>();  
  public forncedorNome = '';  
  public flag: boolean = true;  

  public loading = false;
 
  propostaForm: FormGroup;
  private bodyText: string;

  get nomeProposta() { return this.propostaForm.get('nomeProposta'); }
  get descricao() { return this.propostaForm.get('descricao'); }
  get fornecedor() { return this.propostaForm.get('fornecedor'); }
  get categoria() { return this.propostaForm.get('categoria'); }
  get anexo() { return this.propostaForm.get('anexo'); }
  get valor() { return this.propostaForm.get('valor'); }

  closeResult: string;
  constructor(private propostaService: PropostaService, 
    private router : Router, 
    private fb: FormBuilder,
    private fornecedorService: FornecedorService
  ) {}

  ngOnInit(){
    this.propostaForm =  this.fb.group({
        nomeProposta: new FormControl(null),
        descricao: new FormControl(null),
        fornecedor: new FormControl(null),
        fornecedorID: new FormControl(null),
        categoria: new FormControl(null),
        anexo: new FormControl(null),
        valor: new FormControl(null)
    }, { updateOn: 'submit' });
    this.uploader.nativeElement.value = "";

    this.propostaService.getCategorias()
    .subscribe(data =>{
        this.CategoriaOptions = data.response;
    })

  } 
  onSubmit() {
    console.log('enviar....... ');
    if (this.propostaForm.valid) {
        console.log('is valid');
        this.bodyText = 'This text can be updated in modal 1';
      console.log('this.prepareSaveUpload() ======>>>' , this.prepareSaveUpload());
      

        this.propostaService.createUpload(this.prepareSaveUpload()).subscribe((proposta) => this.created.emit(proposta));
    }
  }
  prepareSaveUpload(): FormData {
    const formModel = this.propostaForm.value;

    console.log('formModel', formModel);
    let formData = new FormData();
    
    formData.append("nomeProposta", formModel.nomeProposta);
    formData.append("anexo", formModel.anexo);
    formData.append("descricao", formModel.descricao);
    formData.append("fornecedorID", formModel.fornecedorID);
    formData.append("categoriaID", formModel.categoria);
    formData.append("valor", formModel.valor);
    
    return formData;
  }
  fileChange(files: FileList) {
      if (files && files[0].size > 0) {
          this.propostaForm.patchValue({
            anexo: files[0]
          });
      }
  }

 search(term: string): void {  
  this.termoFiltro
      .debounceTime(200)
      .switchMap(termo => this.fornecedorService.search(term))
      .subscribe(x => {
        let names = []
          for(let i: number = 0; x['total'] > i; i++){
          names.push({id: x['response'][i].id, nome: x['response'][i].nome});
         } 
         this.fornecedores = names;
      })
      this.termoFiltro.next('');
  this.flag = true;  
  this.termoFiltro.next(term);  
}  
onSelect(fornecedor) {     
    if (fornecedor.id != undefined) {  
      this.propostaForm.patchValue({fornecedor: fornecedor.nome});
      this.propostaForm.patchValue({fornecedorID: fornecedor.id});
      this.flag = false;  
    }  
    else {  
      return false;  
    }  
  } 
}
