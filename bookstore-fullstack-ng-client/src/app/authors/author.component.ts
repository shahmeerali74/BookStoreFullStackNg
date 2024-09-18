import { AsyncPipe, NgIf } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { Author } from "./data/author.model";
import { AuthorListComponent } from "./ui/author-list.component";
import { Store } from "@ngrx/store";
import {
  selectAuthorLoading,
  selectAuthors,
  selectAuthorTotalCount,
} from "./state/author.selectors";
import { Observable } from "rxjs";
import { authorActions } from "./state/author.actions";
import { BookPaginatorComponent } from "./ui/author-paginator.component";
import { PageSelectorModel } from "../common/page-selector.model";

@Component({
  selector: "app-author",
  standalone: true,
  imports: [NgIf, AsyncPipe, AuthorListComponent, BookPaginatorComponent],
  template: `
    <h1>Authors</h1>
    <ng-container *ngIf="authors$ | async as authors">
      <app-author-list
        [authors]="authors"
        (edit)="onEdit($event)"
        (delete)="onDelete($event)"
      />
    </ng-container>

    <app-author-paginator
      (pageSelect)="onPageSelect($event)"
      [totalRecords]="13"
    />
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthorComponent {
  store = inject(Store);
  authors$: Observable<readonly Author[]> = this.store.select(selectAuthors);
  totalCount$ = this.store.select;
  loading$ = this.store.select(selectAuthorLoading);
  error$ = this.store.select(selectAuthorTotalCount);

  onEdit(author: Author) {
    console.log(author);
  }

  onDelete(author: Author) {
    if (
      window.confirm(
        `Are you sure to delete the record (authorName: ${author.authorName})?`
      )
    ) {
      console.log(author);
    }
  }

  onPageSelect(page: PageSelectorModel) {
    console.log(page);
  }
  constructor() {
    this.store.dispatch(authorActions.setCurrentPage({ page: 1 }));
    this.store.dispatch(authorActions.setPageSize({ pageSize: 10 }));
    this.store.dispatch(authorActions.loadAuthors());
  }
}
