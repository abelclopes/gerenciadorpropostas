import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthService } from './logica-api';
import { LoginModule } from './login/login.module';
import { AuthenticationService } from './login/service/authentication.service';
import { AuthGuard } from './_guards/auth.guard';
import { HeaderComponent } from './header/header.component';
import { SharedModule } from './shared/shared.module';
import { NotificationService } from './shared/messages/notification.service';
import { HeaaderService } from './header/header.service';


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LoginModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [
    AuthenticationService,
    AuthGuard,
    NotificationService, HeaaderService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
