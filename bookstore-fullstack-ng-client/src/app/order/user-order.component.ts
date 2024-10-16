import { Store } from "@ngrx/store";
import { userOrderActions } from "./state/user-order/user-order.actions";
import { AsyncPipe, NgIf } from "@angular/common";
import {
  selectUserOrderError,
  selectUserOrderLoading,
  selectUserOrders,
  selectUserOrderTotalCount,
} from "./state/user-order/user-order.selectors";
import { map, Observable } from "rxjs";
import { UserOrderModel } from "./data/user-order.model";
import { MyOrderModel } from "./data/my-order.model";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { UserOrderListComponent } from "./ui/user-order-list.component";
import { PageSelectorModel } from "../common/page-selector.model";
import { AuthorPaginatorComponent } from "../authors/ui/author-paginator.component";

@Component({
  selector: "app-user-order",
  standalone: true,
  imports: [NgIf, AsyncPipe, UserOrderListComponent, AuthorPaginatorComponent],
  template: `
    <ng-container *ngIf="myOrders$ | async as orders">
      <app-user-order-list [userOrders]="orders" />
    </ng-container>

    <ng-container *ngIf="totalCount$ | async as totalCount">
      <app-author-paginator
        (pageSelect)="onPageSelect($event)"
        [totalRecords]="totalCount"
      />
    </ng-container>
  `,
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserOrderComponent {
  private store = inject(Store);
  totalCount$ = this.store.select(selectUserOrderTotalCount);
  loading$ = this.store.select(selectUserOrderLoading);
  error$ = this.store.select(selectUserOrderError);

  userOrders$: Observable<readonly UserOrderModel[]> =
    this.store.select(selectUserOrders);

  myOrders$: Observable<MyOrderModel[]> = this.userOrders$.pipe(
    map((orders: readonly UserOrderModel[]) =>
      orders.flatMap((order) =>
        order.orderItems.map((item) => {
          var myOrderItem: MyOrderModel = {
            orderDate: order.orderDate,
            title: item.book.title,
            imageUrl: item.book.imageUrl ?? "",
            price: item.price,
            authorNames: item.book.authorNames,
            genreNames: item.book.genreNames,
            quantity: item.quantity,
          };
          return myOrderItem;
        })
      )
    )
  );

  loadUserOrders() {
    this.store.dispatch(userOrderActions.getUserOrder());
  }

  onPageSelect(page: PageSelectorModel) {
    // this.store.dispatch(authorActions.setCurrentPage({ page: page.page }));
    // this.store.dispatch(authorActions.setPageSize({ pageSize: page.limit }));
    this.loadUserOrders();
  }
  constructor() {
    this.loadUserOrders();
  }
}
