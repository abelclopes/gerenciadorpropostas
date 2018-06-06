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
import { NotificationService } from '../../../shared/messages/notification.service';
import { UsuariosClans } from '../../usuarios/model/usuario-clans.model';

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
 
  propostaModel: PropostaModel;
  propostaForm: FormGroup;
  private bodyText: string;

  get nomeProposta() { return this.propostaForm.get('nomeProposta'); }
  get descricao() { return this.propostaForm.get('descricao'); }
  get fornecedor() { return this.propostaForm.get('fornecedor'); }
  get categoria() { return this.propostaForm.get('categoria'); }
  get anexo() { return this.propostaForm.get('anexo'); }
  get valor() { return this.propostaForm.get('valor'); }
  get usuario() { return this.propostaForm.get('usuario'); }

  closeResult: string;
  constructor(
    private propostaService: PropostaService, 
    private router : Router, 
    private fb: FormBuilder,
    private fornecedorService: FornecedorService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(){
    this.propostaForm =  this.fb.group({
        nomeProposta: ['', Validators.required, Validators.minLength(5)],
        descricao: new FormControl(null),
        fornecedor: new FormControl(null),
        fornecedorID: new FormControl(null),
        categoria: new FormControl(null),
        anexo: new FormControl(null),
        valor: new FormControl(null),
        usuario: new FormControl(null)
    }, { updateOn: 'submit' });
    this.uploader.nativeElement.value = "";

    this.propostaService.getCategorias()
    .subscribe(data =>{
        this.CategoriaOptions = data.response;
    })

  } 
  onSubmit() {
    if (this.propostaForm.valid) {
        this.bodyText = 'This text can be updated in modal 1';
        this.prepareSaveUpload();
        this.propostaService.createUpload(this.prepareSaveUpload())
        .subscribe(
          data => {
            this.created.emit(data);
            if(data['ok'] == "true")
              this.notificationService.notify(data.response)
              this.router.navigate(['/propostas']);
          }, err => {
            console.log(err);
          }
        );
    }
  }
  prepareSaveUpload(): FormData {
    const formModel = this.propostaForm.value;

    console.log('formModel', formModel);
    let formData = new FormData();
    let usuarioAtual: UsuariosClans = JSON.parse(localStorage.getItem('usuarioClans'));
    console.log("usuarioAtual" ,usuarioAtual);
    formData.append("nomeProposta", formModel.nomeProposta);
    formData.append("anexo", formModel.anexo);
    formData.append("descricao", formModel.descricao);
    formData.append("fornecedorID", formModel.fornecedorID);
    formData.append("categoriaID", formModel.categoria);
    formData.append("valor", formModel.valor);
    formData.append("usuario", usuarioAtual['id'] );
    console.log(formModel.valor)
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
  resetForm(){
    this.propostaForm.reset({
      nomeProposta: this.propostaModel.nomeProposta,
      descricao: this.propostaModel.descricao,
      categoria: this.propostaModel.categoria,
        fornecedor: this.propostaModel.fornecedor,
        valor: this.propostaModel.valor
    });
  }
}
