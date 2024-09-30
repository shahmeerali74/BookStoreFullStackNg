import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Inject,
  Output,
} from "@angular/core";
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { BookCreateModel } from "../Data/book-create.model";
import { Author } from "../../authors/data/author.model";
import { GenreModel } from "../../genre/data/genre.model";

@Component({
  selector: "app-book-dialog",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
  ],
  template: `
    <h1 mat-dialog-title>
      {{ data.title }}
    </h1>

    <div mat-dialog-content>
      <form [formGroup]="bookForm" class="author-form">
        <input type="hidden" formControlName="id" />
        <mat-form-field appearance="fill">
          <mat-label>Title</mat-label>
          <input type="text" matInput formControlName="title" />
        </mat-form-field>

        <mat-form-field appearance="fill">
          <mat-label>Description</mat-label>
          <input type="text" matInput formControlName="description" />
        </mat-form-field>

        <mat-form-field appearance="fill">
          <mat-label>Price</mat-label>
          <input type="number" matInput formControlName="price" />
        </mat-form-field>

        <mat-form-field appearance="fill">
          <mat-label>Published Year</mat-label>
          <input type="text" matInput formControlName="publishedYear" />
        </mat-form-field>

        <div mat-dialog-actions>
          <button
            mat-raised-button
            color="primary"
            (click)="onSubmit()"
            [disabled]="bookForm.invalid"
            cdkFocusInitial
          >
            Save
          </button>
          <button mat-raised-button color="warn" (click)="onCanceled()">
            Close
          </button>
        </div>
      </form>
    </div>
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookDialogComponent {
  @Output() submit = new EventEmitter<BookCreateModel>();
  bookForm: FormGroup = new FormGroup({
    id: new FormControl<number>(0),
    title: new FormControl<string>("", Validators.required),
    description: new FormControl<string>("", Validators.required),
    price: new FormControl<number>(0),
    publishedYear: new FormControl<number>(0),
    authorIds: new FormControl<number[]>([]),
    genreIds: new FormControl<number[]>([]),
  });

  onCanceled() {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.bookForm.valid) {
      const book: BookCreateModel = Object.assign(this.bookForm.value);
      this.submit.emit(book);
    }
  }

  constructor(
    public dialogRef: MatDialogRef<BookDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      book: BookCreateModel | null;
      title: string;
      authors: Array<Author>;
      genres: Array<GenreModel>;
    }
  ) {
    if (data.book != null) {
      this.bookForm.patchValue(data.book);
    }
  }
}
