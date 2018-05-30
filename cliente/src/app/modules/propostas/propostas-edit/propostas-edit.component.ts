import { Component, Input, OnChanges, ElementRef, OnInit, EventEmitter, Output, ViewChild, ViewEncapsulation  } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { PropostaModel, PropostaAnexoModel, PropostaNovaModel } from '../model';
import { PropostaService } from '../service/proposta.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { LoadingService } from '../../../LoadingService';
import { FornecedorModel } from '../../fornecedores/model';
import { FornecedorService } from '../../fornecedores/service/fornecedor.service';
import { Observable, Subject } from 'rxjs';  
import { NotificationService } from '../../../shared/messages/notification.service';

@Component({
  moduleId: module.id.toString(),
  selector: 'app-propostas-edit',
  templateUrl: './propostas-edit.component.html',
  styleUrls: ['./propostas-edit.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class PropostasEditComponent implements OnInit {
  inputChanged: string;
  selectedItem: any;
  
  @Input() proposta: PropostaModel;
  @Output() created: EventEmitter<PropostaModel> = new EventEmitter<PropostaModel>();
  @Output() updated: EventEmitter<PropostaModel> = new EventEmitter<PropostaModel>();
  //@ViewChild("uploader") uploader: ElementRef;
  @ViewChild('uploader') uploader;

  CategoriaOptions: {id:number, nome:string}[];

  public fornecedores: any[] = [];
  private termoFiltro = new Subject<string>();  
  public forncedorNome = '';  
  public flag: boolean = true;  

  fileTmp: File;

  public loading = false;

  propostaModel: PropostaModel;
  propostaAnexo: PropostaAnexoModel;
  propostaForm: FormGroup;

  get nomeProposta() { return this.propostaForm.get('nomeProposta'); }
  get descricao() { return this.propostaForm.get('descricao'); }
  get fornecedor() { return this.propostaForm.get('fornecedor'); }
  get categoria() { return this.propostaForm.get('categoria'); }
  get anexo() { return this.propostaForm.get('anexo'); }
  get valor() { return this.propostaForm.get('valor'); }

  closeResult: string;
  constructor(
    private propostaService: PropostaService, 
    private router : Router, 
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private fornecedorService: FornecedorService,    
    private fb: FormBuilder) { }

  ngOnInit() {
    this.propostaForm =  this.fb.group({
      nomeProposta: new FormControl(null),
      descricao: new FormControl(null),
      fornecedor: new FormControl(null),
      fornecedorID: new FormControl(null),
      categoria: new FormControl(null),
      anexo: new FormControl(null),
      valor: new FormControl(null)
  }, { updateOn: 'submit' });
  
  //this.uploader.nativeElement.value = "";

    this.propostaService.getPropostaById(this.route.snapshot.paramMap.get('id'))
    .subscribe(
      data => {
        this.propostaModel = data;
        console.log(this.propostaModel)
      }, err => {
        console.log(err);
      });
      
    this.propostaService.getCategorias()
    .subscribe(data =>{
        this.CategoriaOptions = data.response;
    })

  }
  // ngAfterViewInit(){

  //  console.log("afterinit");
  //  console.log(this.uploader.nativeElement.value);

  // }
  
  onSubmit() {
    //if (this.propostaForm.valid) {
        this.propostaService.updateProposta(this.route.snapshot.paramMap.get('id'),this.prepareSaveUpload())
        .subscribe(
          data => {
            this.created.emit(data);
            if(data['ok'] == "true")
              this.notificationService.notify(data['response'])
              this.router.navigate(['/propostas']);
          }, err => {
            console.log(err);
          }
        );
    // }else{
    //   console.log('invalido')
    // }
  }
  prepareSaveUpload(): PropostaNovaModel {
    const formModel = this.propostaForm.value;
    let p = new PropostaNovaModel();

    console.log('=====================const ', this.propostaModel);
    p.nomeProposta = formModel.nomeProposta;
    p.fornecedorID = formModel.fornecedorID;
    p.categoriaID = formModel.categoria;
    p.fornecedorID = formModel.fornecedorID;
    p.valor = formModel.valor;
    if(this.fileTmp.size > 0){
      p.files = this.fileTmp;
      p.anexo = this.fileTmp.name
    }
    if(this.propostaModel)
      p.id = this.propostaModel.id;
    console.log('=====================const2 ', p);
    
    let formData = new FormData();
    
    formData.append("nomeProposta", formModel.nomeProposta);
    formData.append("descricao", formModel.descricao);
    formData.append("fornecedorID", formModel.fornecedorID);
    formData.append("categoriaID", formModel.categoria);
    formData.append("valor", formModel.valor);
    if(this.fileTmp.size > 0){
      p.files = this.fileTmp;
      formData.append("anexo", this.fileTmp);
    }

    return p;
  }
  
  fileChange() {
    const fileBrowser = this.uploader.nativeElement;
    
    if (fileBrowser.files && fileBrowser.files[0]) {
      let files: File = fileBrowser.files[0];
        this.fileTmp = fileBrowser.files[0];
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
