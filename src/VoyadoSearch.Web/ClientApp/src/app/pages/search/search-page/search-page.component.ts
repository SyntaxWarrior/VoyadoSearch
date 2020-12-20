import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { VoyadoSearchAPI } from 'src/app/api/voyado-search-api';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss']
})
export class SearchPageComponent {

  _loading = true;
  _searchEngines: VoyadoSearchAPI.SearchEngineContract[];
  
  // the main form group for search text
  _searchTextForm: FormGroup;
  // a second form group for holding possible search engines to search with.
  _searchEngineForm: FormGroup;

  _searching = false;
  _searchHitsTotal = 0;
  _searchPerformed = false;
  _searchResults: VoyadoSearchAPI.SearchResultContract | null;

  constructor(
      private http: HttpClient,
      private formBuilder: FormBuilder,
    ) { 

      const api = new VoyadoSearchAPI.EngineVoyadoClient(this.http);
      api.list().subscribe(list => {
      this._searchEngines = list;
      this.CreateForm();
      this._loading = false;
    });
  }

  CreateForm(){
    this._searchTextForm = this.formBuilder.group({
      searchText: [],
    });

    this._searchEngineForm = this.formBuilder.group({});

    this._searchEngines.forEach(engine => {
      console.log(engine);
      this._searchEngineForm.addControl("" + engine.id, new FormControl(true, null));
    });
  }

  EngineControlNames(): Array<string> { 
    let result = new Array<string>();
    for (const field in this._searchEngineForm.controls) {
      result.push(field);
    }
    return result;
  }

  GetEnabledSearchEngines(): Array<string> {
    let result = new Array<string>();
    for (const field in this._searchEngineForm.controls) {
      var enabled = this._searchEngineForm.controls[field].value;
      if(enabled) {
        result.push(field);
      }
    }
    return result;
  }

  GetControl(name: string): FormControl{
    return this._searchEngineForm.controls[name] as FormControl;
  }

  ForceSearch(term: string){
    console.log("force: " + term);
    this._searchTextForm.controls.searchText.setValue(term);
    this.OnSearch();
  }

  OnSearch(){
      const engines = this.GetEnabledSearchEngines();
      const terms = this._searchTextForm.controls.searchText.value

      console.log(engines);
      console.log(terms);

      const api = new VoyadoSearchAPI.SearchVoyadoClient(this.http);
      
      this._searching = true;

      api.query(terms, engines).subscribe(response => {
        this._searchPerformed = true;

        if(response != null && response.results != null)  {
          this._searchResults = response;
        }else{
          this._searchResults = null;
        }

        this._searching = false;
      }, error => {
        this._searching = false;
      });
  }
}