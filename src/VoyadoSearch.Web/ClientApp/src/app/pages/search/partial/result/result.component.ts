import { Component, Input, OnInit } from '@angular/core';
import { VoyadoSearchAPI } from 'src/app/api/voyado-search-api';

@Component({
  selector: 'app-search-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent  {
  @Input() searchResults: VoyadoSearchAPI.SearchResultContract;
  constructor() { }
}
