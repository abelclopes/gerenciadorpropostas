import { NgModule } from '@angular/core';
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
import { HeaderService } from './components/header/header.service';
import { PaginationFilter } from './shared/pagination-filter.pipe';
import { LoginModule } from './components/login/login.module';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { LoadingService } from './LoadingService';
import { MaskDirective } from './shared/diretivas/mask.directive';
import { ModalModule } from 'ngx-bootstrap';

import { CustomCurrencyMaskConfig } from './shared/diretivas/custon-currency-masck-config';


import { NgxCurrencyModule } from "ngx-currency";
import { CURRENCY_MASK_CONFIG } from 'ngx-currency/src/currency-mask.config';

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
    NgxCurrencyModule
  ],
  providers: [
    AuthenticationService,
    AuthGuard,
    NotificationService, HeaderService,
    PaginationFilter, LoadingService,
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
