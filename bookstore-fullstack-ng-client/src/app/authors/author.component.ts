import { AsyncPipe, NgIf } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { Author } from "./data/author.model";
import { AuthorListComponent } from "./ui/author-list.component";
import { Store } from "@ngrx/store";
import {
  selectAuthorError,
  selectAuthorLoading,
  selectAuthors,
} from "./state/author.selectors";
import { Observable, tap } from "rxjs";
import { authorActions } from "./state/author.actions";

@Component({
  selector: "app-author",
  standalone: true,
  imports: [NgIf, AsyncPipe, AuthorListComponent],
  template: `
    <h1>Authors</h1>
    <ng-container *ngIf="authors$ | async as authors">
      <app-author-list
        [authors]="authors"
        (edit)="onEdit($event)"
        (delete)="onDelete($event)"
      />
    </ng-container>
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthorComponent {
  store = inject(Store);
  authors$: Observable<readonly Author[]> = this.store.select(selectAuthors);

  loading$ = this.store.select(selectAuthorLoading);
  error$ = this.store.select(selectAuthorError);

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
  constructor() {
    this.store.dispatch(authorActions.loadAuthors());
  }
}
