import { inject, Injectable } from "@angular/core";
import { CartService } from "../data/cart.service";
import { Actions } from "@ngrx/effects";

@Injectable()
export class CartEffects {
  cartService = inject(CartService);
  actions$ = inject(Actions);
}
