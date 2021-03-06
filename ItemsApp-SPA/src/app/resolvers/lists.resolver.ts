import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Item } from '../models/item';
import { ItemService } from '../services/item.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class ListsResolver implements Resolve<Item[]>{
    pageNumber = 1;
    pageSize = 5;
    constructor(private itemService: ItemService, private alertifyService: AlertifyService,
         private router: Router,  private spinnerService: NgxSpinnerService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Item[]> {
        this.spinnerService.show('spAllItems');
        return this.itemService.getItems(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertifyService.error(error);
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
