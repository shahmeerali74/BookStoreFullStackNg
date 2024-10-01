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
import { NgSelectModule } from "@ng-select/ng-select";
import { Observable } from "rxjs";
import { AsyncPipe } from "@angular/common";

@Component({
  selector: "app-book-dialog",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    NgSelectModule,
    AsyncPipe,
  ],
  template: `
    <h1 mat-dialog-title>
      {{ data.title }}
    </h1>

    <div mat-dialog-content>
      <form [formGroup]="bookForm" class="book-form">
        <input type="hidden" formControlName="id" />
        <input type="hidden" formControlName="imageUrl" />
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
          <input type="number" matInput formControlName="publishedYear" />
        </mat-form-field>

        <input type="file" (change)="onChange($event)" />

        <ng-select
          [items]="data.authors$ | async"
          [multiple]="true"
          [closeOnSelect]="false"
          [hideSelected]="true"
          bindLabel="authorName"
          bindValue="id"
          placeholder="Select Authors"
          formControlName="authorIds"
        >
        </ng-select>

        <ng-select
          [items]="data.genres$ | async"
          [multiple]="true"
          [closeOnSelect]="false"
          [hideSelected]="true"
          bindLabel="genreName"
          bindValue="id"
          placeholder="Select Genres"
          formControlName="genreIds"
        >
        </ng-select>

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
  styles: [
    `
      mat-form-field,
      ng-select {
        width: 300px;
      }

      .book-form {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 16px;
      }
    `,
  ],
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
    imageUrl: new FormControl<string>(""),
    imageFile: new FormControl<File | null>(null),
  });

  onCanceled() {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.bookForm.valid) {
      const book: BookCreateModel = Object.assign(this.bookForm.value);
      console.log(book);
      // this.submit.emit(book);
    }
  }

  onChange(event: any) {
    this.bookForm.get("imageFile")?.setValue(event.target.files[0]);
  }

  constructor(
    public dialogRef: MatDialogRef<BookDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      book: BookCreateModel | null;
      title: string;
      authors$: Observable<Array<Author>>;
      genres$: Observable<Array<GenreModel>>;
    }
  ) {
    if (data.book != null) {
      this.bookForm.patchValue(data.book);
    }
  }
}
