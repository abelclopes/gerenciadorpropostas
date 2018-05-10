import { Injectable } from '@angular/core';
import { Http, Headers, HttpModule, Response, RequestMethod, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
import { API_URL } from '../../../app.api';
import { CategoriasModel } from '../../../logica-api';
import { CategoriaPagedListModel } from '../model';

@Injectable()
export class CategoriaService {

    httpHeaders: HttpHeaders;

    constructor(protected httpClient: HttpClient, private http: Http) {

      var currentUser = JSON.parse(localStorage.getItem('usuarioCorrente'));
       this.httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('No-Auth', 'true')
      .set('Authorization', `Bearer ${currentUser.token}`)
      .set('x-access-token', `${currentUser.token}`);
    }

    public getCategorias(pagina, tamanho, buscaTermo?: any): Observable<CategoriaPagedListModel> {
      if(buscaTermo == undefined){ buscaTermo ='';}
      return this.httpClient.get<CategoriaPagedListModel>(`${API_URL}/api/categorias?PageNumber=${pagina}&PageSize=${tamanho}&buscaTermo=${buscaTermo}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public novaCategoria(nome: string, descricao: string){
      let data = { "nome": nome,  "descricao": descricao};
      return this.httpClient.post<CategoriasModel>(`${API_URL}/api/categorias/`,data,{headers: this.httpHeaders, responseType: 'json'});
    }
    public getCategoriaById(id: string): Observable<CategoriasModel> {
      return this.httpClient.get<CategoriasModel>(`${API_URL}/api/categorias/${id}`,{
        headers: this.httpHeaders, responseType: 'json'
      });
    }
    public updateCategoria(model: CategoriasModel): Observable<CategoriaPagedListModel> {
      let data = { "nome": model.nome,  "descricao": model.descricao};
      let url = `${API_URL}/api/categorias/${model.id}/?nome=${model.nome}&descricao=${model.descricao}`;
      return this.httpClient.put(url,null,
        {
          headers: this.httpHeaders
        }
      );
    }
    public delete(id: string): any {
      let url = `${API_URL}/api/categorias/${id}`;
      return this.httpClient.delete(url, { headers: this.httpHeaders } );
    }
}
