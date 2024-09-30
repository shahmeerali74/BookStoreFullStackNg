import { Component, inject, OnInit } from "@angular/core";
import { BookListComponent } from "./ui/book-list.component";
import { Store } from "@ngrx/store";
import { BookActions } from "./state/book.actions";
import { selectBooks } from "./state/book.selectors";
import { AsyncPipe, NgIf } from "@angular/common";

@Component({
  selector: "app-book",
  standalone: true,
  imports: [BookListComponent, NgIf, AsyncPipe],
  template: `
    <ng-container *ngIf="books$ | async as books">
      <app-book-list [books]="books"></app-book-list>
    </ng-container>
  `,

  styles: [``],
})
export class BookComponent implements OnInit {
  store = inject(Store);

  books$ = this.store.select(selectBooks);

  ngOnInit(): void {
    this.store.dispatch(BookActions.loadBooks());
  }
}
