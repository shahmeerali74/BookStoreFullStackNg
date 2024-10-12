import { AsyncPipe, JsonPipe, NgFor, NgIf } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { Store } from "@ngrx/store";
import { CartActions } from "./state/cart.actions";
import { Observable } from "rxjs";
import { CartItem } from "./data/cart-read.model";
import {
  selectCartError,
  selectCartItems,
  selectCartLoading,
} from "./state/cart.selectors";
import { HttpErrorResponse } from "@angular/common/http";
import { CartItemComponent } from "./ui/cart-item.component";

@Component({
  selector: "app-cart",
  standalone: true,
  imports: [NgIf, NgFor, AsyncPipe, CartItemComponent],
  template: `
    <div class="cart-container">
      <ng-container *ngIf="loading$ | async"> loading... </ng-container>
      <!-- if not error then #no-orror -->
      <ng-container *ngIf="error$ | async; else noerror">
        Error occured
      </ng-container>
      <ng-template #noerror>
        <ng-container *ngIf="cartItems$ | async as cartItems">
          <ng-container *ngIf="cartItems.length > 0; else noitems">
            <div class="cart-items">
              <app-cart-item
                *ngFor="let cartItem of cartItems; trackBy: trackById"
                [cartItem]="cartItem"
                (selectQuantity)="onSelectQuantity($event)"
                (deleteItem)="onDeleteItem($event)"
              />
            </div>
            <!-- <app-cart-summary
              [subTotal]="(subTotal$ | async) ?? 0"
              [tax]="(tax$ | async) ?? 0"
              [total]="(total$ | async) ?? 0"
            /> -->
          </ng-container>

          <ng-template #noitems>
            <h3>No items in the cart 🛒. Please <a href="/books">add ➕</a></h3>
          </ng-template>
        </ng-container>
      </ng-template>
    </div>
  `,
  styles: [
    `
      .cart-container {
        display: flex;
        gap: 20px;
        padding: 15px;
      }

      .cart-items {
        width: 62%;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CartComponent {
  private readonly store = inject(Store);
  cartItems$: Observable<ReadonlyArray<CartItem>> =
    this.store.select(selectCartItems);
  loading$: Observable<boolean> = this.store.select(selectCartLoading);
  error$: Observable<HttpErrorResponse | null> =
    this.store.select(selectCartError);

  trackById(index: number, cartItem: CartItem) {
    return cartItem.id;
  }

  private loadCartItems() {
    this.store.dispatch(CartActions.loadCartItems());
  }

  onSelectQuantity(data: { cartItem: CartItem; newQuantity: number }) {
    console.log({ cartItem: data.cartItem, newQuantity: data.newQuantity });
  }

  onDeleteItem(id: number) {
    console.log(id);
  }

  constructor() {
    this.loadCartItems();
  }
}
