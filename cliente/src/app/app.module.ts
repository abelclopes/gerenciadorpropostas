import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


/* App Root */
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';

import { AppRoutingModule } from './/app-routing.module';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { SpinnerComponent } from './spinner/spinner.component';

/* Account Imports */
import { AccountModule }  from './account/account.module';
/* Dashboard Imports */
import { DashboardModule }  from './dashboard/dashboard.module';

import { ConfigService } from './shared/utils/config.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardModule,
    HomeComponent,
    RegistrationFormComponent,
    SpinnerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
