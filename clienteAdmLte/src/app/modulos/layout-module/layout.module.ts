import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { SideLeftBarComponent } from './side-left-bar/side-left-bar.component';
import { FooterComponent } from './footer/footer.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    HeaderComponent, 
    SideLeftBarComponent, 
    FooterComponent
  ],
  exports:[
    HeaderComponent, 
    SideLeftBarComponent, 
    FooterComponent
  ]
})
export class LayoutModule { }
