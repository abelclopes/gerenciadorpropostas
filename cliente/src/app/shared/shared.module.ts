import {NgModule, ModuleWithProviders} from '@angular/core'
import {CommonModule} from '@angular/common'
import {FormsModule, ReactiveFormsModule} from '@angular/forms'

import {InputComponent} from './input/input.component'

import { SnackbarComponent } from './messages/snackbar/snackbar.component';

import {NotificationService} from './messages/notification.service';
import { ANNOTATIONS } from '@angular/core/src/util/decorators';

import { NgxCurrencyModule } from "ngx-currency";
import { CURRENCY_MASK_CONFIG } from 'ngx-currency/src/currency-mask.config';

import { GeproMaskUtilService, MaskDirective } from './diretivas';
import { PaginationFilter } from './pagination-filter.pipe';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
@NgModule({
  declarations: [
    InputComponent, 
    SnackbarComponent,
    MaskDirective,
    PageNotFoundComponent
  ],
  imports: [
    CommonModule, 
    FormsModule, 
    ReactiveFormsModule,
    NgxCurrencyModule
  ],
  exports: [
    CommonModule,
    InputComponent, 
    SnackbarComponent,
    FormsModule, 
    ReactiveFormsModule,
    MaskDirective
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers:[
        NotificationService,
        PaginationFilter,
        GeproMaskUtilService
      ]
    }
  }
}
