import { Component, OnInit } from '@angular/core';
import { PropostaModel, PropostaAnexoModel } from '../model';
import { PropostaService } from '../service/proposta.service';
import { Router, ActivatedRoute } from '@angular/router';
import {DomSanitizer,SafeResourceUrl} from '@angular/platform-browser'

import { LoadingService } from '../../../LoadingService';
import * as FileSaver from 'file-saver'; 
import { PDFProgressData } from 'pdfjs-dist';

@Component({
  selector: 'app-proposta-detalhes',
  templateUrl: './proposta-detalhes.component.html',
  styleUrls: ['./proposta-detalhes.component.css']
})
export class PropostaDetalhesComponent implements OnInit {
  propostaModel: PropostaModel;
  propostaAnexo: PropostaAnexoModel
  arquivo: any;

  displayPdf: boolean;
  page: number = 1;
  totalPages: number;
  isLoaded: boolean = false;
  dataLocalUrl:any;
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
    this.propostaService.getPropostaById(this.route.snapshot.paramMap.get('id'))
    .subscribe(
      data => {
        this.propostaModel = data;
        //console.log(this.propostaModel)
      }, err => {
        console.log(err);
      });
    
  }
  editar(){
    this.router.navigate([`/propostas/editar/${this.route.snapshot.paramMap.get('id')}`]);
  }
  loadAnexo(){
    if(this.displayPdf){
      this.displayPdf = false;
    }else{
      this.propostaService.getPropostaArquivo(this.route.snapshot.paramMap.get('id'))
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
