import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, RequestMethod, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch';

import { of } from 'rxjs/observable/of';
import 'rxjs/add/operator/delay';

import { PropostaPagedListModel, PropostaModel} from '../model';
import { API_URL } from '../../../app.api';
import { MyResponse } from '../../../logica-api/model/my-response';

@Injectable()
export class PropostaService {

    httpHeaders: HttpHeaders;

    constructor(protected httpClient: HttpClient, private http: Http) {

      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
      this.httpHeaders = new HttpHeaders()
      .set('Accept', 'Application/json')
      .set('Content-Type', 'Application/json')
      //.set('Content-Type', 'application/x-www-form-urlencoded')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
    }

    public getPropostas(pagina, tamanho, buscaTermo?: any): Observable<PropostaPagedListModel> {
      if(buscaTermo == undefined){ buscaTermo ='';}
      return this.httpClient.get<PropostaPagedListModel>(`${API_URL}/api/propostas?PageNumber=${pagina}&PageSize=${tamanho}&buscaTermo=${buscaTermo}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public novoProposta(model: PropostaModel){
      let data = JSON.stringify(model);
      return this.httpClient.post<PropostaModel>(`${API_URL}/api/propostas/`,model,{headers: this.httpHeaders, responseType: 'json'});
    }
    public getPropostaById(id: string): Observable<PropostaModel> {
      return this.httpClient.get<PropostaModel>(`${API_URL}/api/propostas/${id}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public updateProposta(model: PropostaModel): Observable<PropostaPagedListModel> {
      let data = JSON.stringify(model);
      let url = `${API_URL}/api/propostas/${model.id}`;
      return this.httpClient.put(url,data,
        {
          headers: this.httpHeaders
        }
      );
    }
    public delete(id: string): any {
      let url = `${API_URL}/api/propostas/${id}`;
      return this.httpClient.delete(url, { headers: this.httpHeaders } );
    }
    
    public getStatus(): any {
      let url = `${API_URL}/api/propostas/status`;
      return this.httpClient.get(url, { headers: this.httpHeaders } );
  
    }

    public createUpload(model: FormData): Observable<any> {
      // debugger;
      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
      var currentUserDetails = JSON.parse(localStorage.getItem('userDetails'));
      model.append("usuario", currentUserDetails.id);
      console.log(model);
      
      let httpHeaders = new HttpHeaders()
     // .set('Content-Type', 'Application/json;')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
      let url = `${API_URL}/api/propostas`;
      return this.httpClient.post(url, model,{headers: httpHeaders})
      //.map(this.extractObject);
    } 

    
    getCategorias(): any {
      let url = `${API_URL}/api/categorias/getall`;
      return this.httpClient.get(url, { headers: this.httpHeaders } );
    }

    private extractObject(res: Response): Object {
      const data: any = res.json();
      return data || {};
    }
}
