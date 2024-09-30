import { Component, inject, OnInit } from "@angular/core";
import { BookListComponent } from "./ui/book-list.component";
import { Store } from "@ngrx/store";
import { BookActions } from "./state/book.actions";
import { selectBooks } from "./state/book.selectors";
import { AsyncPipe, NgIf } from "@angular/common";
import { BookReadModel } from "./Data/book-read.model";
import { Observable } from "rxjs";
import { SortModel } from "../common/sort.model";

@Component({
  selector: "app-book",
  standalone: true,
  imports: [BookListComponent, NgIf, AsyncPipe],
  template: `
    <ng-container *ngIf="books$ | async as books">
      <app-book-list
        [books]="books"
        (sort)="onSort($event)"
        (delete)="onDelete($event)"
        (edit)="onAddUpdate('Update', $event)"
      ></app-book-list>
    </ng-container>
  `,

  styles: [``],
})
export class BookComponent implements OnInit {
  store = inject(Store);

  books$: Observable<ReadonlyArray<BookReadModel>> =
    this.store.select(selectBooks);

  loadBooks() {
    this.store.dispatch(BookActions.loadBooks());
  }

  ngOnInit(): void {
    this.loadBooks();
  }

  onSort(sort: SortModel) {
    this.store.dispatch(
      BookActions.setSortBy({
        sortBy: `${sort.sortColumn} ${sort.sortDirection}`,
      })
    );
    this.loadBooks();
  }

  onAddUpdate(action: string, book: BookReadModel | null = null) {}

  onDelete(book: BookReadModel) {
    if (
      window.confirm(`Are you sure to delete the record (book: ${book.title})?`)
    ) {
      this.store.dispatch(BookActions.deleteBook({ id: book.id }));
      this.loadBooks();
    }
  }
}
