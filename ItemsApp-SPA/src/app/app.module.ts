import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule} from 'ngx-bootstrap/modal';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { NgxSpinnerModule } from "ngx-spinner";
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { appRoutes } from './routes';
import { ListsResolver } from './resolvers/lists.resolver';

@NgModule({
   declarations: [	
      AppComponent,
      NavComponent,
      HomeComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      RouterModule.forRoot(appRoutes),
      PaginationModule.forRoot(),
      ModalModule.forRoot(),
      ButtonsModule,
      NgxSpinnerModule
   ],
   providers: [
      ListsResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule {
}
