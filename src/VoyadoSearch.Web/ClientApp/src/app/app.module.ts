import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http'; 

import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopMenuComponent } from './partial/top-menu/top-menu.component';
import { FooterComponent } from './partial/footer/footer.component';
import { SearchPageComponent } from './pages/search/search-page/search-page.component';
import { HistoryListComponent } from './pages/search/partial/history-list/history-list.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { HelpComponent } from './pages/help/help.component';
import { AboutComponent } from './pages/about/about.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule }   from '@angular/forms';
import { HistoricComponent } from './pages/search/partial/historic/historic.component';
import { ResultComponent } from './pages/search/partial/result/result.component';

@NgModule({
  declarations: [
    AppComponent,
    TopMenuComponent,
    FooterComponent,
    SearchPageComponent,
    HistoryListComponent,
    PageNotFoundComponent,
    HelpComponent,
    AboutComponent,
    HistoricComponent,
    ResultComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    library.addIconPacks(fas, far);
  }
 }
