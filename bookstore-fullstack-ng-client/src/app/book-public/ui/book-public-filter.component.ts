import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Output,
} from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormControl, ReactiveFormsModule } from "@angular/forms";
import { MatFormField } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { debounceTime, tap } from "rxjs";

@Component({
  selector: "app-book-public-filter",
  standalone: true,
  imports: [MatFormField, MatInputModule, ReactiveFormsModule],
  template: `
    <div class="book-filters">
      <mat-form-field appearance="outline">
        <mat-label>Filter by Title/Author</mat-label>
        <input matInput [formControl]="searchTerm" />
      </mat-form-field>
    </div>
  `,
  styles: [
    `
      .book-filters {
        padding-top: 10px;
        padding-left: 96px;
        width: 100%;
      }
      mat-form-field {
        width: 500px;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookPublicFilterComponent {
  searchTerm = new FormControl("");
  @Output() onSearch = new EventEmitter<string | null>();

  constructor() {
    this.searchTerm.valueChanges
      .pipe(
        debounceTime(400),
        tap((sTerm) => {
          this.onSearch.emit(sTerm);
        }),
        takeUntilDestroyed()
      )
      .subscribe();
  }
}
