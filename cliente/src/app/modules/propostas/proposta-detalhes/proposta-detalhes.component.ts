import { Component, OnInit } from '@angular/core';
import { PropostaModel, PropostaAnexoModel, PropSituacao, PropSituacaoResponse } from '../model';
import { PropostaService } from '../service/proposta.service';
import { Router, ActivatedRoute } from '@angular/router';
import {DomSanitizer,SafeResourceUrl} from '@angular/platform-browser'

import { LoadingService } from '../../../LoadingService';
import * as FileSaver from 'file-saver'; 
import { PDFProgressData } from 'pdfjs-dist';
import { propostaStaus } from '../model/proposta-status.model';

@Component({
  selector: 'app-proposta-detalhes',
  templateUrl: './proposta-detalhes.component.html',
  styleUrls: ['./proposta-detalhes.component.css']
})
export class PropostaDetalhesComponent implements OnInit {

  abilitarAprovar: boolean = false;
  propostaModel: PropostaModel;
  propostaAnexo: PropostaAnexoModel
  arquivo: any;
  propostaStatus: propostaStaus;
  propostaSituacao: PropSituacaoResponse;

  displayPdf: boolean;
  page: number = 1;
  totalPages: number;
  isLoaded: boolean = false;
  dataLocalUrl:any;
  
  id = this.route.snapshot.paramMap.get('id');
  constructor(
    private propostaService: PropostaService, 
    private router : Router, 
    private route: ActivatedRoute,
    private domSanitizer: DomSanitizer,
    public loadingService: LoadingService
  ) 
  {
    this.loadingService.showLoading()
   }

  ngOnInit() {
    this.propostaService.getPropostaById(this.id)
    .subscribe(
      data => {
        this.propostaModel = data;
        //console.log(this.propostaModel)
      }, err => {
        console.log(err);
      });
      this.propostaService.getStatus(this.id)
      .subscribe(
        data => {
          this.propostaStatus = data.response.propSituacao;
          this.abilitarAprovar = this.tomadaDeDecisaoAbilitarBotao(data);

        }, err => {
          console.log(err);
        });
    
  }
  
  
  tomadaDeDecisaoAbilitarBotao(data: PropSituacaoResponse): boolean {
    switch(data.response.usuario.perfil)
    {
      case 3:
          if(data.response.propSituacao.status == 1 
            && data.response.propSituacao.valorPropostaAcimaDoLimiteDesMill 
            && data.response.propSituacao.aprovadaDiretorFinanceiro == false){
              console.log("nao pode aprovar, ainda nao aprovado por diretor financeiro");
            return false;
          }else if(data.response.propSituacao.status == 1 
            && data.response.propSituacao.valorPropostaAcimaDoLimiteDesMill 
            && data.response.propSituacao.aprovadaDiretorFinanceiro){
              this.propostaModel.status = 2
            return true;
          }else if(data.response.propSituacao.status == 1 && data.response.propSituacao.valorPropostaAcimaDoLimiteDesMill == false){
            this.propostaModel.status = 2
            return true;
          }
      break;
      case 4:
          if(data.response.propSituacao.status == 1             
            && data.response.propSituacao.valorPropostaAcimaDoLimiteDesMill 
            && data.response.propSituacao.aprovadaAnalistaFinanceiro == false
            && data.response.propSituacao.aprovadaDiretorFinanceiro == false){              
            console.log("aprovar por diretor financeiro valorPropostaAcimaDoLimiteDesMill == true");
            this.propostaModel.status = 4
            return true;
          } else if(data.response.propSituacao.status == 1             
            && data.response.propSituacao.valorPropostaAcimaDoLimiteDesMill == false
            && data.response.propSituacao.aprovadaDiretorFinanceiro == false
            && data.response.propSituacao.aprovadaAnalistaFinanceiro == false
          ){
            this.propostaModel.status = 4;
            console.log("aprovar por diretor financeiro valorPropostaAcimaDoLimiteDesMill == false");
            return true;
          } 
      break;
      default:
        return false;
    }
    return false;
  }


  editar(){
    this.router.navigate([`/propostas/editar/${this.id}`]);
  }
  loadAnexo(){
    if(this.displayPdf){
      this.displayPdf = false;
    }else{
      this.propostaService.getPropostaArquivo(this.id)
      .subscribe(
        data => {
          this.propostaAnexo = data
          this.arquivo =  this.base64ToArrayBuffer(this.propostaAnexo.fileContent);     
          this.loadingService.hideLoading()   
          this.displayPdf = true;
        }, err => {
          console.log(err);
      });
    }
  }
  aprovarProposta(){
    this.propostaService.AprovarProposta(this.id, this.propostaModel)
    .subscribe(
      data => {
       if(data['ok'] == true){
         this.abilitarAprovar = false;
       }
      }, err => {
        console.log(err);
    });
  }
  validaAbilitarAprovar(){
    this.propostaService.getPropostaPermissaoAprovar(this.id)
    .subscribe(
      data => {
        data;
        console.log(data);
        this.loadingService.hideLoading()   
        this.abilitarAprovar = true;
      }, err => {
        console.log(err);
    });
  }

  base64ToArrayBuffer(base64) {
    let binary_string =  window.atob(base64);
    let len = binary_string.length;
    let bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++)        {
        bytes[i] = binary_string.charCodeAt(i);
    }
    return bytes.buffer;
  }
  savePdf(data:PropostaAnexoModel){
    var blob = this.b64toBlob(data.fileContent, data.contentType, 512);
    var blobUrl = URL.createObjectURL(blob);
    
    let filename = `${data.nome}.pdf`;
    FileSaver.saveAs(blob, filename);
  }
  b64toBlob(b64Data, contentType, sliceSize:number) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;
  
    let byteCharacters = atob(b64Data);
    let byteArrays = [];
  
    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      let slice = byteCharacters.slice(offset, offset + sliceSize);
  
      let byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
  
      let byteArray = new Uint8Array(byteNumbers);
  
      byteArrays.push(byteArray);
    }
  
    let blob = new Blob(byteArrays, {type: contentType});
    return blob;
  }

  afterLoadComplete(pdfData: any) {
    this.totalPages = pdfData.numPages;
    this.isLoaded = true;
  }
  onProgress(progressData: PDFProgressData) {
    this.loadingService.showLoading()
  }
  nextPage() {
    this.page++;
  }

  prevPage() {
    this.page--;
  }
}
