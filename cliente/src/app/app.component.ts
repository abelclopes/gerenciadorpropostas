import { Component } from '@angular/core';
import { LoadingService } from './LoadingService';
import * as $ from 'jquery';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  loading = false;

  OnInit(){
    
  }
 
  
}
