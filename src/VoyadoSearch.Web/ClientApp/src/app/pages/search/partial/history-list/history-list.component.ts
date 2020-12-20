import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { VoyadoSearchAPI } from 'src/app/api/voyado-search-api';

@Component({
  selector: 'app-search-history-list',
  templateUrl: './history-list.component.html',
  styleUrls: ['./history-list.component.scss']
})
export class HistoryListComponent {

  @Output() searchEvent = new EventEmitter<string>();
  
  _loading = true;
  _previousSearches: VoyadoSearchAPI.SearchHistoryContract[];

  constructor(private http: HttpClient) { 
    this.ReloadData();
  }

  private ReloadData() {
    const api = new VoyadoSearchAPI.SearchVoyadoClient(this.http);
    api.history().subscribe(data => {
      this._previousSearches = data;
      this._loading = false;
    });
  }

  searchFor(paramater: VoyadoSearchAPI.SearchHistoryContract){
    //this.SearchTerm(paramater.term);
    this.searchEvent.emit(paramater.term);
  }
}
