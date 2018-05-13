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

import { LoadingBarModule } from '@ngx-loading-bar/core';

import { AutocompleteModule } from 'ng2-input-autocomplete';

import { LoadingModule, ANIMATION_TYPES } from 'ngx-loading';

// import { CoreModule } from './core/core.module';
import { LoadingService } from './LoadingService';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    HeaderComponent,
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
    LoadingBarModule.forRoot(),
    LoadingModule.forRoot({
      animationType: ANIMATION_TYPES.wanderingCubes,
      backdropBackgroundColour: 'rgba(0,0,0,1)', 
      backdropBorderRadius: '4px',
      primaryColour: '#ffffff', 
      secondaryColour: '#ffffff', 
      tertiaryColour: '#ffffff'
  }),
  AutocompleteModule.forRoot()
  ],
  providers: [
    AuthenticationService,
    AuthGuard,
    NotificationService, HeaderService,
    PaginationFilter, LoadingService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
