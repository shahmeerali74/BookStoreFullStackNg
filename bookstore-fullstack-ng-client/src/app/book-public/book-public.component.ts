import { AsyncPipe, NgFor, NgIf } from "@angular/common";
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnDestroy,
} from "@angular/core";
import { BookReadModel } from "../book/Data/book-read.model";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Store } from "@ngrx/store";
import {
  selectBookError,
  selectBookLoading,
  selectBooks,
} from "../book/state/book.selectors";
import { Subject } from "rxjs";
import { BookListPublicComponent } from "./ui/book-list-public.component";
import { BookActions } from "../book/state/book.actions";

@Component({
  selector: "app-book-public",
  standalone: true,
  imports: [NgIf, NgFor, AsyncPipe, BookListPublicComponent],
  template: `
    <ng-container *ngIf="books$ | async as books">
      <app-book-list-public
        [books]="books"
        (addToCart)="addToCart($event)"
      ></app-book-list-public>
    </ng-container>
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookPublicComponent implements OnDestroy {
  snackBar = inject(MatSnackBar);
  store = inject(Store);
  books$ = this.store.select(selectBooks);
  loading$ = this.store.select(selectBookLoading);
  error$ = this.store.select(selectBookError);
  destroy$ = new Subject<boolean>();

  addToCart(book: BookReadModel) {
    throw new Error("Method not implemented.");
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

  constructor() {
    this.store.dispatch(BookActions.setPageSize({ pageSize: 1000 }));
    this.store.dispatch(BookActions.loadBooks());
  }
}
