/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.9.4.0 (NJsonSchema v10.3.1.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export module VoyadoSearchAPI {
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class EngineVoyadoClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    /**
     * @return Success
     */
    list(): Observable<SearchEngineContract[]> {
        let url_ = this.baseUrl + "/engine/list";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "text/plain"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processList(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processList(<any>response_);
                } catch (e) {
                    return <Observable<SearchEngineContract[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<SearchEngineContract[]>><any>_observableThrow(response_);
        }));
    }

    protected processList(response: HttpResponseBase): Observable<SearchEngineContract[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(SearchEngineContract.fromJS(item));
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<SearchEngineContract[]>(<any>null);
    }
}

@Injectable()
export class SearchVoyadoClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    /**
     * @param searchTerms (optional) 
     * @param body (optional) 
     * @return Success
     */
    query(searchTerms: string | null | undefined, body: string[] | null | undefined): Observable<SearchResultContract> {
        let url_ = this.baseUrl + "/search/query?";
        if (searchTerms !== undefined && searchTerms !== null)
            url_ += "searchTerms=" + encodeURIComponent("" + searchTerms) + "&";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "text/plain"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processQuery(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processQuery(<any>response_);
                } catch (e) {
                    return <Observable<SearchResultContract>><any>_observableThrow(e);
                }
            } else
                return <Observable<SearchResultContract>><any>_observableThrow(response_);
        }));
    }

    protected processQuery(response: HttpResponseBase): Observable<SearchResultContract> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = SearchResultContract.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<SearchResultContract>(<any>null);
    }

    /**
     * @return Success
     */
    history(): Observable<SearchHistoryContract[]> {
        let url_ = this.baseUrl + "/search/history";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "text/plain"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processHistory(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processHistory(<any>response_);
                } catch (e) {
                    return <Observable<SearchHistoryContract[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<SearchHistoryContract[]>><any>_observableThrow(response_);
        }));
    }

    protected processHistory(response: HttpResponseBase): Observable<SearchHistoryContract[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(SearchHistoryContract.fromJS(item));
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<SearchHistoryContract[]>(<any>null);
    }
}

export class SearchEngineContract implements ISearchEngineContract {
    readonly id?: string | undefined;
    readonly displayName?: string | undefined;

    constructor(data?: ISearchEngineContract) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            (<any>this).id = _data["id"];
            (<any>this).displayName = _data["displayName"];
        }
    }

    static fromJS(data: any): SearchEngineContract {
        data = typeof data === 'object' ? data : {};
        let result = new SearchEngineContract();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["displayName"] = this.displayName;
        return data; 
    }
}

export interface ISearchEngineContract {
    id?: string | undefined;
    displayName?: string | undefined;
}

export class SearchEngineResultContract implements ISearchEngineResultContract {
    readonly engineId?: string | undefined;
    readonly displayName?: string | undefined;
    readonly searchTerm?: string | undefined;
    readonly hitCount?: number;

    constructor(data?: ISearchEngineResultContract) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            (<any>this).engineId = _data["engineId"];
            (<any>this).displayName = _data["displayName"];
            (<any>this).searchTerm = _data["searchTerm"];
            (<any>this).hitCount = _data["hitCount"];
        }
    }

    static fromJS(data: any): SearchEngineResultContract {
        data = typeof data === 'object' ? data : {};
        let result = new SearchEngineResultContract();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["engineId"] = this.engineId;
        data["displayName"] = this.displayName;
        data["searchTerm"] = this.searchTerm;
        data["hitCount"] = this.hitCount;
        return data; 
    }
}

export interface ISearchEngineResultContract {
    engineId?: string | undefined;
    displayName?: string | undefined;
    searchTerm?: string | undefined;
    hitCount?: number;
}

export class SearchResultContract implements ISearchResultContract {
    readonly totalHits?: number;
    readonly elapsedMilliseconds?: number;
    readonly results?: SearchEngineResultContract[] | undefined;

    constructor(data?: ISearchResultContract) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            (<any>this).totalHits = _data["totalHits"];
            (<any>this).elapsedMilliseconds = _data["elapsedMilliseconds"];
            if (Array.isArray(_data["results"])) {
                (<any>this).results = [] as any;
                for (let item of _data["results"])
                    (<any>this).results!.push(SearchEngineResultContract.fromJS(item));
            }
        }
    }

    static fromJS(data: any): SearchResultContract {
        data = typeof data === 'object' ? data : {};
        let result = new SearchResultContract();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalHits"] = this.totalHits;
        data["elapsedMilliseconds"] = this.elapsedMilliseconds;
        if (Array.isArray(this.results)) {
            data["results"] = [];
            for (let item of this.results)
                data["results"].push(item.toJSON());
        }
        return data; 
    }
}

export interface ISearchResultContract {
    totalHits?: number;
    elapsedMilliseconds?: number;
    results?: SearchEngineResultContract[] | undefined;
}

export class SearchHistoryContract implements ISearchHistoryContract {
    readonly searchDate?: Date;
    readonly term?: string | undefined;

    constructor(data?: ISearchHistoryContract) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            (<any>this).searchDate = _data["searchDate"] ? new Date(_data["searchDate"].toString()) : <any>undefined;
            (<any>this).term = _data["term"];
        }
    }

    static fromJS(data: any): SearchHistoryContract {
        data = typeof data === 'object' ? data : {};
        let result = new SearchHistoryContract();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["searchDate"] = this.searchDate ? this.searchDate.toISOString() : <any>undefined;
        data["term"] = this.term;
        return data; 
    }
}

export interface ISearchHistoryContract {
    searchDate?: Date;
    term?: string | undefined;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}

}