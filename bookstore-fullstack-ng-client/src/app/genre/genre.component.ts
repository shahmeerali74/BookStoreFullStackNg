import { ChangeDetectionStrategy, Component, Injectable } from "@angular/core";
import { GenreListComponent } from "./ui/genre-list.component";
import { GenreModel } from "./data/genre.model";
import { MatButtonModule } from "@angular/material/button";
@Component({
  selector: "app-book",
  standalone: true,
  imports: [GenreListComponent, MatButtonModule],
  template: `
    <h1>Genre</h1>
    <p>
      <button mat-raised-button color="primary" (click)="({})">
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
}
