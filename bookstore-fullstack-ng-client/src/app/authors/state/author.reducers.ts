import { HttpErrorResponse } from "@angular/common/http";
import { Author } from "../data/author.model";
import { authorActions } from "./author.actions";
import { createReducer, on } from "@ngrx/store";

export const authorFeaturKey = "author";

export interface AuthorState {
  authors: ReadonlyArray<Author>;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  loading: boolean;
  error: HttpErrorResponse | null;
}

export const initialState: AuthorState = {
  authors: [],
  totalCount: 0,
  pageNumber: 1,
  pageSize: 10,
  totalPages: 0,
  hasPrevious: false,
  hasNext: false,
  loading: false,
  error: null,
};

export const genreReducers = createReducer(
  initialState,
  on(authorActions.loadAuthors, (state) => ({
    ...state,
    loading: true,
  })),
  on(authorActions.loadAuthorsSucess, (state, { authors }) => ({
    ...state,
    loading: false,
    authors,
  })),
  on(authorActions.loadAuthorsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),
  on(authorActions.addAuthor, (state, { author }) => ({
    ...state,
    loading: true,
  })),
  on(authorActions.addAuthorSuccess, (state, { author }) => ({
    ...state,
    loading: false,
    authors: [...state.authors, author],
  })),
  on(authorActions.addAuthorFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  }))
);
