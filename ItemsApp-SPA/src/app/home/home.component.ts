import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Item } from '../models/item';
import { Pagination, PaginatedResult } from '../models/pagination';
import { AlertifyService } from '../services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { ItemService } from '../services/item.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  items: Item[];
  pagination: Pagination;
  isEditMode = false;
  addEditModalRef: BsModalRef;
  deleteModalRef: BsModalRef;
  itemForm: FormGroup;
  selectedItemId: number;
  selectedItem: Item;
  showAllItems = true;
  maxPriceForItem: number;
  itemNames: string[];
  constructor(private formBuilder: FormBuilder, private modalService: BsModalService,
    private route: ActivatedRoute, private itemService: ItemService,
    private alertifyService: AlertifyService, private spinnerService: NgxSpinnerService) { }

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.items = data.items.result;
      this.pagination = data.items.pagination;
      this.spinnerService.hide('spAllItems');
    });

    this.createItemForm();
    this.getItemNames();

  }



  openAddEditModal(template: TemplateRef<any>, item?: Item, editMode?: boolean) {
    this.selectedItem = Object.assign({}, item);
    this.isEditMode = editMode;
    this.itemForm.reset();
    this.addEditModalRef = this.modalService.show(template);
  }

  openDeleteModal(template: TemplateRef<any>, selectedItemId: number) {
    this.selectedItemId = selectedItemId;
    this.deleteModalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  decline(): void {
    this.deleteModalRef.hide();
  }

  createItemForm() {
    let numbersOnlyPattern = '^[0-9]*$';
    this.itemForm = this.formBuilder.group({
      itemName: ['', Validators.required],
      cost: ['', [
        Validators.required,
        Validators.pattern(numbersOnlyPattern)
      ]]
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    if (this.showAllItems)
      this.getItems();
    else
      this.getMaxPricesForItems();
  }

  getItems() {
    this.showAllItems = true;
    this.spinnerService.show('spAllItems');
    this.itemService.getItems(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<Item[]>) => {
      this.items = res.result;
      this.pagination = res.pagination;
      this.spinnerService.hide('spAllItems');
    }, error => {
      this.alertifyService.error(error);
    });
  }

  getMaxPricesForItems() {
    this.showAllItems = false;
    this.spinnerService.show('spAllItems');
    this.itemService.getMaxPricesForItems(this.pagination.currentPage,
      this.pagination.itemsPerPage).subscribe((res: PaginatedResult<Item[]>) => {
        this.items = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide('spAllItems');
      }, error => {
        this.alertifyService.error(error);
      });
  }

  GetMaxPriceForItem(itemName: string) {
    this.itemService.getMaxPriceForItem(itemName).subscribe((result: number) => {
      this.maxPriceForItem = result;
    }, error => {
      this.alertifyService.error(error);
    });
  }

  getItemNames() {
    this.itemService.getItemNames().subscribe((result: string[]) => {
      this.itemNames = result;
    }, error => {
      this.alertifyService.error(error);
    });
  }

  deleteItem() {

    this.itemService.deleteItem(this.selectedItemId).subscribe(() => {
      this.alertifyService.success('Item Deleted Successfully');
      this.deleteModalRef.hide();
    }, error => {
      this.alertifyService.error(error);
    }, () => {
      this.pagination.currentPage = 1;
      this.getItems();
      this.getItemNames();
    });

  }



  saveItem() {
    if (this.itemForm.valid) {
      if (this.isEditMode) {
        this.spinnerService.show('spAddEditItem');
        this.itemService.updateItem(this.selectedItem.id, this.selectedItem).subscribe(() => {
          this.spinnerService.hide('spAddEditItem');
          this.alertifyService.success('Item Updated Successfully');
          this.addEditModalRef.hide();
        }, error => {
          this.alertifyService.error(error);
        }, () => {
          this.pagination.currentPage = 1;
          this.getItems();
        });
      }
      else {
        this.spinnerService.show('spAddEditItem');
        this.itemService.createItem(this.selectedItem).subscribe(() => {
          this.spinnerService.hide('spAddEditItem');
          this.alertifyService.success('Item Created Successfully');
          this.addEditModalRef.hide();
        }, error => {
          this.alertifyService.error(error);
        }, () => {
          this.pagination.currentPage = 1;
          this.getItems();
          this.getItemNames();
        });
      }
    }
  }


}
