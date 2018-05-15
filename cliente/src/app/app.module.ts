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
import { KzMaskDirective } from './shared/mascara.directive';
import { NgxCurrencyModule } from "ngx-currency";
import { CURRENCY_MASK_CONFIG } from 'ngx-currency/src/currency-mask.config';

import { CustomCurrencyMaskConfig } from './shared/diretivas/custon-currency-masck-config';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    HeaderComponent,
    KzMaskDirective,
    PaginationFilter
  ],
  imports: [
    NgbModule.forRoot(),
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LoginModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    NgxDatatableModule,
    HttpClientModule,
    NgxPaginationModule,
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
