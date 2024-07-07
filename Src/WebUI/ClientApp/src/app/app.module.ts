import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AppRoutingModule } from './app.routing.module';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ProductsComponent } from './products/products.component';
import { CustomersComponent } from './customers/customers.component';
import { NavTopMenuComponent } from './nav-top-menu/nav-top-menu.component';
import { NavSideMenuComponent } from './nav-side-menu/nav-side-menu.component';

import { CustomersClient, ProductsClient, API_BASE_URL } from './northwind-traders-api';

import { CamelCaseToText } from '../pipes/camel-case-to-text';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';

@NgModule({ declarations: [
        AppComponent,
        NavTopMenuComponent,
        NavSideMenuComponent,
        HomeComponent,
        ProductsComponent,
        CustomersComponent,
        CamelCaseToText
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        FormsModule,
        AppRoutingModule,
        ApiAuthorizationModule], providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
        CustomersClient,
        ProductsClient,
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
