import {
  ChangeDetectionStrategy,
  Component,
  inject,
  Injectable,
} from "@angular/core";
import { GenreListComponent } from "./ui/genre-list.component";
import { GenreModel } from "./data/genre.model";
import { MatButtonModule } from "@angular/material/button";
import { MatDialog } from "@angular/material/dialog";
import { Subject, takeUntil } from "rxjs";
import { AddGenreDialogComponent } from "./ui/add-genre-dialog.component";
@Component({
  selector: "app-book",
  standalone: true,
  imports: [GenreListComponent, MatButtonModule],
  template: `
    <h1>Genre</h1>
    <p>
      <button mat-raised-button color="primary" (click)="onAddUpdate('Add')">
        Add Genre
      </button>
    </p>
    <app-genre-list
      [genres]="genres"
      (edit)="onEdit($event)"
      (delete)="onDelete($event)"
    />
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GenreComponent {
  dialog = inject(MatDialog);
  destroyed$ = new Subject<boolean>();
  genres: GenreModel[] = [
    { id: 1, genreName: "Horror" },
    { id: 2, genreName: "Action" },
    { id: 3, genreName: "Comedy" },
    { id: 4, genreName: "Drama" },
    { id: 5, genreName: "Romance" },
    { id: 6, genreName: "Thriller" },
  ];

  onEdit(genre: GenreModel) {
    console.log(genre);
  }

  onDelete(genre: GenreModel) {
    console.log(genre);
  }

  onAddUpdate(action: string, genre: GenreModel | null = null) {
    const dialogRef = this.dialog.open(AddGenreDialogComponent, {
      data: { genre, title: action + " Genre" },
    });

    dialogRef.componentInstance.submit
      .pipe(takeUntil(this.destroyed$))
      .subscribe((submittedBook) => {
        console.log(submittedBook);
        if (!submittedBook) return;
        if (submittedBook.id) {
          // update book
        } else {
        }
        // TODO: lines below only executed, when we have added books successfully
        dialogRef.componentInstance.genreForm.reset();
        dialogRef.componentInstance.onCanceled();
      });
  }
}
