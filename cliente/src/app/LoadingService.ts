import { Injectable } from '@angular/core';

@Injectable()
export class LoadingService {
  
  public loading: boolean = false;

  showLoading(){
    this.loading = true;
  }

  hideLoading(){
    this.loading = false;
  }

}