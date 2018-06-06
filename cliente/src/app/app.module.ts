import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import {NgxPaginationModule} from 'ngx-pagination';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthService } from './logica-api';
import { AuthenticationService } from './components/login/service/authentication.service';
import { AuthGuard } from './_guards/auth.guard';
import { HeaderComponent } from './components/header/header.component';
import { SharedModule } from './shared/shared.module';
import { NotificationService } from './shared/messages/notification.service';
import { PaginationFilter } from './shared/pagination-filter.pipe';
import { LoginModule } from './components/login/login.module';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { LoadingModule } from 'ngx-loading';
import { LoadingService } from './LoadingService';
import { MaskDirective } from './shared/diretivas/mask.directive';
import { ModalModule } from 'ngx-bootstrap';
import { LoadingModule, ANIMATION_TYPES } from 'ngx-loading';

import { CustomCurrencyMaskConfig } from './shared/diretivas/custon-currency-masck-config';

import { CurrencyMaskModule } from "ng2-currency-mask";

import { CURRENCY_MASK_CONFIG } from 'ngx-currency/src/currency-mask.config';
import { GeproMaskUtilService } from './shared/diretivas';

import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import localeptExtra from '@angular/common/locales/extra/pt';
registerLocaleData(localePt, 'pt', localeptExtra);

import { TextMaskModule } from 'angular2-text-mask';


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    HeaderComponent,
    PaginationFilter
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgxDatatableModule,
    HttpClientModule,
    NgxPaginationModule,
    NgbModule.forRoot(),
    SharedModule.forRoot(),
    ModalModule.forRoot(),
    LoadingModule.forRoot({
      animationType: ANIMATION_TYPES.wanderingCubes,
      backdropBackgroundColour: 'rgba(0,0,0,0.1)', 
      backdropBorderRadius: '4px',
      primaryColour: '#ffffff', 
      secondaryColour: '#ffffff', 
      tertiaryColour: '#ffffff'
  }),
    NgxCurrencyModule
  ],
  providers: [
    AuthenticationService,
    AuthGuard,
    NotificationService,
    PaginationFilter, LoadingService,
    GeproMaskUtilService,
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig },
    { provide: LOCALE_ID, useValue: 'pt' } ,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
