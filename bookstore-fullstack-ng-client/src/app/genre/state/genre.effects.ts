import { Injectable } from "@angular/core";
import { act, Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap } from "rxjs";
import { Store } from "@ngrx/store";
import { GenreService } from "../data/genre.service";
import { GenreActions } from "./genre.actions";

@Injectable()
export class GenreEffects {
  loadBooks = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.loadGenre),
      switchMap(() =>
        this.genreService.getGenres().pipe(
          map((genres) => GenreActions.loadGenreSuccess({ genres })),
          catchError((error) => of(GenreActions.loadGenreFailure({ error })))
        )
      )
    )
  );

  addBook = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.addGenre),
      switchMap((action) => {
        return this.genreService.addGenre(action.genre).pipe(
          map((genre) => GenreActions.addGenreSuccess({ genre })),
          catchError((error) => of(GenreActions.addGenreFailure({ error })))
        );
      })
    )
  );

  updateBook = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.updateGenre),
      switchMap((action) => {
        return this.genreService.updateGenre(action.genre).pipe(
          map((genre) => GenreActions.updateGenreSuccess({ genre })),
          catchError((error) => of(GenreActions.updateGenreFailure({ error })))
        );
      })
    )
  );

  deleteBook = createEffect(() =>
    this.actions$.pipe(
      ofType(GenreActions.deleteGenre),
      switchMap((action) =>
        this.genreService.deleteGenre(action.id).pipe(
          map(() => GenreActions.deleteGenreSuccess({ id: action.id })),
          catchError((error) => of(GenreActions.deleteGenreFailure({ error })))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private store: Store,
    private genreService: GenreService
  ) {}
}
