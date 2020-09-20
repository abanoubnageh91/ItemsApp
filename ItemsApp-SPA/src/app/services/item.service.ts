import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Item } from '../models/item';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ItemService {
  baseUrl: string = environment.apiUrl + 'Items/';
  constructor(private http: HttpClient) { }

  createItem(item: Item) {
    return this.http.post(this.baseUrl, item);
  }

  deleteItem(id: number) {
    return this.http.delete(this.baseUrl + id);
  }

  updateItem(id: number, item: Item) {
    return this.http.put(this.baseUrl + id, item);
  }

  getItems(pageNumber?, pageSize?): Observable<PaginatedResult<Item[]>> {
    const paginatedResult: PaginatedResult<Item[]> = new PaginatedResult<Item[]>();

    let params = new HttpParams();
    if (pageNumber != null && pageSize != null) {
      params = new HttpParams(
        {
          fromObject: { pageNumber, pageSize }
        }
      );
    }

    return this.http.get<Item[]>(this.baseUrl + 'GetItems', {
      observe: 'response',
      params
    }).pipe(
      map((response) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getMaxPricesForItems(pageNumber?, pageSize?): Observable<PaginatedResult<Item[]>> {
    const paginatedResult: PaginatedResult<Item[]> = new PaginatedResult<Item[]>();

    let params = new HttpParams();
    if (pageNumber != null && pageSize != null) {
      params = new HttpParams(
        {
          fromObject: { pageNumber, pageSize }
        }
      );
    }

    return this.http.get<Item[]>(this.baseUrl + 'GetMaxPricesForItems', {
      observe: 'response',
      params
    }).pipe(
      map((response) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getMaxPriceForItem(itemName: string): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'GetMaxPriceForItem/' + itemName);
  }

  getItemNames(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'GetItemNames');
  }

}


